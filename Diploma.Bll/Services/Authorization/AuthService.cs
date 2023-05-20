using System;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Diploma.Bll.Common.Exceptions;
using Diploma.Bll.Common.Providers.Encryption.Curves;
using Diploma.Bll.Common.Providers.Encryption.Keys;
using Diploma.Bll.Common.Response;
using Diploma.Bll.Services.Access;
using Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Request.Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Response;
using Diploma.Bll.Services.Encryption;
using Diploma.Bll.Services.Token;
using Diploma.Ecc.Math.Entities;
using Diploma.Ecc.Math.Extensions;
using Diploma.Persistence;
using Diploma.Persistence.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Diploma.Bll.Services.Authorization
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        private readonly ITokenService _tokenService;
        private readonly IKeysProvider _keysProvider;
        private readonly ICurveProvider _curveProvider;
        private readonly ICryptoService _cryptoService;
        private readonly IAccessManager _accessManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="curveProvider">Провайдер кривой</param>
        /// <param name="keysProvider">Провайдер ключей шифрования</param>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="cryptoService">Сервис шифрования</param>
        /// <param name="tokenService">Сервис токена</param>
        /// <param name="accessManager">Сервис доступа</param>
        public AuthService(
            DatabaseContext context, 
            IKeysProvider keysProvider,
            ICurveProvider curveProvider,
            IConfiguration configuration, 
            ICryptoService cryptoService,
            ITokenService tokenService,
            IAccessManager accessManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
            _keysProvider = keysProvider ?? throw new ArgumentNullException(nameof(keysProvider));
            _curveProvider = curveProvider ?? throw new ArgumentNullException(nameof(curveProvider));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        /// <summary>
        /// Обменяться ключами шифрования
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        public async Task<ExchangeKeysResponse> ExchangeKeysAsync(ExchangeKeysRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var curve = _curveProvider.GetCurve();

            var userPublicKey = new Point(
                BigInteger.Parse(request.PublicKey.X), 
                BigInteger.Parse(request.PublicKey.Y), 
                curve);
            
            var userPrivateKey = await _context.Users
                .Where(x => x.Id == _accessManager.UserId)
                .Select(x => x.PrivateKey)
                .FirstOrDefaultAsync();

            var serverPublicKey = new Point(
                BigInteger.Parse(_configuration.GetSection("Encryption:PublicKey:X").Value),
                BigInteger.Parse(_configuration.GetSection("Encryption:PublicKey:Y").Value),
                curve);
            
            var serverPrivateKey = BigInteger.Parse(_configuration.GetSection("Encryption:PrivateKey").Value);

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
        public async Task<UserAuthResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == request.Email.Trim().ToLower()))
            {
                throw new RequestException(HttpStatusCode.Conflict, "Пользователь уже существует");
            }

            var encryptionKey = BigInteger.Parse(_configuration.GetSection("Encryption:PrivateKey").Value);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Name.Trim(),
                Email = request.Email.Trim(),
                PasswordHash = _cryptoService.Encrypt(request.Password, encryptionKey.ToByteArray())
            };
            
            _context.Users.Add(user);

            var curve = _curveProvider.GetCurve();
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
                Token = _tokenService.Generate(user.Id)
            };
        }

        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        public async Task<UserAuthResponse> LoginUserAsync(LoginUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.Trim().ToLower());
            if (user == null)
            {
                throw new RequestException(HttpStatusCode.Conflict, "Пользователь не существует");
            }
            
            var encryptionKey = BigInteger.Parse(_configuration.GetSection("Encryption:PrivateKey").Value);
            var encryptedPassword = _cryptoService.Encrypt(request.Password, encryptionKey.ToByteArray());
            if (user.PasswordHash != encryptedPassword)
            {
                throw new RequestException(HttpStatusCode.Forbidden, "Доступ запрещен");
            }

            return new UserAuthResponse
            {
                Token = _tokenService.Generate(user.Id)
            };
        }
    }
}