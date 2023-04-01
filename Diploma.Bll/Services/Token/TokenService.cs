using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Diploma.Bll.Common.Exceptions;
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

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

            var securityToken = new JwtSecurityToken
            (
                issuer: _configuration["Token:Issuer"],
                claims: new[]
                {
                    new Claim("id", userId.ToString())
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