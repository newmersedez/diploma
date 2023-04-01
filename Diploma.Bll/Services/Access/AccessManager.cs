using System;
using System.Linq;
using Diploma.Bll.Services.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Diploma.Bll.Services.Access
{
    /// <summary>
    /// Сервис управления доступом
    /// </summary>
    public sealed class AccessManager : IAccessManager
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="tokenService">Сервис управления токеном</param>
        /// <param name="httpContextAccessor">Доступ к контексту</param>
        public AccessManager(
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId
        {
            get
            {
                if (Token == null)
                {
                    var header = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization]
                        .FirstOrDefault();

                    if (header != null)
                    {
                        Token = header.Replace("Bearer", "");
                    }
                }

                return _tokenService.GetUserId(Token);
            }
        }

        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }
}