using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Diploma.ECC.Math.Extensions;
using Diploma.Persistence;
using Diploma.Persistence.Models.Entities;
using Diploma.Server.Common.Responses;
using Diploma.Server.Services.Authorization.Request;
using Diploma.Server.Services.Authorization.Response;
using Diploma.Server.Services.Crypto;
using Diploma.Server.Services.KeysProvider;
using Diploma.Storage.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Expressions;

namespace Diploma.Server.Services.Authorization
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        private readonly ICryptoService _cryptoService;
        private readonly IKeysProvider _keysProvider;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="cryptoService">Сервис шифрования</param>
        /// <param name="keysProvider">Провайдер ключей шифрования</param>
        public AuthService(
            DatabaseContext context, 
            IConfiguration configuration, 
            ICryptoService cryptoService,
            IKeysProvider keysProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
            _keysProvider = keysProvider ?? throw new ArgumentNullException(nameof(keysProvider));
        }

        /// <summary>
        /// Обменяться ключами шифрования
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        public async Task<ExchangeKeysResponse> ExchangeKeysAsync(ExchangeKeysRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: верификация запроса
            
            var curveName = _configuration.GetValue<string>("Encryption:Curve");
            var curveEnum = (CurveName)Enum.Parse(typeof(CurveName), curveName);
            var curve = new Curve(curveEnum);

            var userPublicKey = new Point(
                BigInteger.Parse(request.PublicKey.X), 
                BigInteger.Parse(request.PublicKey.Y), 
                curve);
            
            var userPrivateKey = await _context.Users
                .Where(x => x.Id == request.Id)
                .Select(x => x.PrivateKey)
                .FirstOrDefaultAsync();

            var serverPublicKey = new Point(
                BigInteger.Parse(_configuration.GetValue<string>("Encryption:PublicKey:X")),
                BigInteger.Parse(_configuration.GetValue<string>("Encryption:PublicKey:Y")),
                curve);
            
            var serverPrivateKey = BigInteger.Parse(_configuration.GetValue<string>("Encryption:PrivateKey"));

            var sharedKey = userPublicKey.Multiply(serverPrivateKey);
            var encryptionKey = new Rfc2898DeriveBytes(
                sharedKey.ToString()!, new byte[] {0, 0, 0, 0, 0, 0, 0, 0}, 1000, HashAlgorithmName.SHA256);

            var encryptedUserPrivateKey = _cryptoService.Encrypt(userPrivateKey.Key, encryptionKey.GetBytes(32));

            return new ExchangeKeysResponse
            {
                ServerPublicKey = new PublicKeyInfo
                {
                    X = serverPublicKey.X.ToString(),
                    Y = serverPublicKey.Y.ToString()
                },
                UserPrivateKey = encryptedUserPrivateKey
            };
        }

        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        public async Task<UserAuthResponse> RegisterUserAsync(UserAuthRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: Верификация запроса

            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == request.Email.Trim().ToLower()))
            {
                throw new RequestException(HttpStatusCode.Conflict, "Пользователь уже существует");
            }

            var encryptionKey = BigInteger.Parse(_configuration.GetValue<string>("Encryption:PrivateKey"));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email.Trim(),
                PasswordHash = _cryptoService.Encrypt(request.Password, encryptionKey.ToByteArray())
            };
            
            _context.Users.Add(user);

            var curveName = _configuration.GetValue<string>("Encryption:Curve");
            var curveEnum = (CurveName)Enum.Parse(typeof(CurveName), curveName);
            var curve = new Curve(curveEnum);
            var keys = _keysProvider.CreateKeyPair(curve);

            var userPublicKey = new UserPublicKey
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                X = keys.PublicKey.X.ToString(),
                Y = keys.PublicKey.Y.ToString()
            };
            _context.UserPublicKeys.Add(userPublicKey);
            
            var userPrivateKey = new UserPrivateKey
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Key = keys.PrivateKey.ToString()
            };
            _context.UserPrivateKeys.Add(userPrivateKey);
            
            await _context.SaveChangesAsync();
            
            return new UserAuthResponse
            {
                Id = user.Id
            };
        }

        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        public async Task<UserAuthResponse> LoginUserAsync(UserAuthRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: Верификация запроса
            
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.Trim().ToLower());
            if (user == null)
            {
                throw new RequestException(HttpStatusCode.NotFound, "Пользователь не существует");
            }
            
            var encryptionKey = BigInteger.Parse(_configuration.GetValue<string>("Encryption:PrivateKey"));
            var encryptedPassword = _cryptoService.Encrypt(request.Password, encryptionKey.ToByteArray());
            if (user.PasswordHash != encryptedPassword)
            {
                throw new RequestException(HttpStatusCode.Forbidden, "Доступ запрещен");
            }

            return new UserAuthResponse
            {
                Id = user.Id
            };
        }
    }
}