using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Diploma.Bll.Common.Exceptions;
using Diploma.Persistence;
using Diploma.Persistence.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.Bll.Services.Token
{
    /// <summary>
    /// Сервис управления токеном
    /// </summary>
    public sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="context"></param>
        public TokenService(IConfiguration configuration, DatabaseContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public static TokenValidationParameters TokenValidationParameters(IConfiguration configuration)
        {
            var rsa = RSA.Create();
            
            rsa.ImportRSAPublicKey(Convert.FromBase64String(configuration["Token:Key:Public"]), out _);

            var securityKey = new RsaSecurityKey(rsa);

            return new()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Token:Issuer"],
                ValidateAudience = false,
                ValidateLifetime = false,
                IssuerSigningKey = securityKey,
                ValidateIssuerSigningKey = true
            };
        }
        
        /// <summary>
        /// Генерация токена пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        public string Generate(Guid userId)
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(_configuration["Token:Key:Private"]), out _);

            var credentials = new SigningCredentials
            (
                new RsaSecurityKey(rsa),
                SecurityAlgorithms.RsaSha256
            );

            var user = _context.Users.Include(x => x.PublicKey).FirstOrDefault(x => x.Id == userId);
            if (user is null) return null;

            var securityToken = new JwtSecurityToken
            (
                issuer: _configuration["Token:Issuer"],
                claims: new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("email", user.Email),
                    new Claim("username", user.Username),
                    new Claim("publicX", user.PublicKey.X),
                    new Claim("publicY", user.PublicKey.Y),

                },
                signingCredentials: credentials
            );

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(securityToken);

            return accessToken;
        }

        /// <summary>
        /// Получить данные из токена
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns></returns>
        public Guid GetUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "id");
            if (userId == null)
            {
                throw new RequestException(HttpStatusCode.Unauthorized, "Токен не содержит нужную информацию");
            }

            return new Guid(userId.Value);
        }
    }
}