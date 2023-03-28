using System;
using System.Threading.Tasks;
using Diploma.Server.Services.Authorization;
using Diploma.Server.Services.Authorization.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Server.Controllers
{
    /// <summary>
    /// Контроллер авторизации пользователей
    /// </summary>
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="authService">Сервис авторизации</param>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        [HttpPost("/register")]
        public async Task RegisterUserAsync([FromServices] IAuthService authService, UserAuthRequest request)
        {
            if (authService == null) throw new ArgumentNullException((nameof(authService)));

            await authService.RegisterUserAsync(request);
        }
        
        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="authService">Сервис авторизации</param>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("/login")]
        public async Task LoginUserAsync([FromServices] IAuthService authService, UserAuthRequest request)
        {
            if (authService == null) throw new ArgumentNullException((nameof(authService)));

            await authService.LoginUserAsync(request);
        }
    }
}