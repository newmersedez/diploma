using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Authorization;
using Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Request.Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Server.Controllers
{
    /// <summary>
    /// Контроллер авторизации пользователей
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Обменяться публичными ключами
        /// </summary>
        /// <param name="authService">Сервис управления доступом</param>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("exchange-keys")]
        public async Task<ExchangeKeysResponse> ExchangeKeysAsync(
            [FromServices] IAuthService authService,
            [FromBody] ExchangeKeysRequest request
            )
        {
            if (authService == null) throw new ArgumentNullException(nameof(authService));

            var response = await authService.ExchangeKeysAsync(request);

            return response;
        }
    
        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="authService">Сервис авторизации</param>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<UserAuthResponse> RegisterUserAsync(
            [FromServices] IAuthService authService, 
            [FromBody] RegisterUserRequest request)
        {
            if (authService == null) throw new ArgumentNullException((nameof(authService)));

            var response = await authService.RegisterUserAsync(request);

            return response;
        }
        
        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="authService">Сервис авторизации</param>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<UserAuthResponse> LoginUserAsync(
            [FromServices] IAuthService authService, 
            [FromBody] LoginUserRequest request)
        {
            if (authService == null) throw new ArgumentNullException((nameof(authService)));

            var response = await authService.LoginUserAsync(request);

            return response;
        }
    }
}