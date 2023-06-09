using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Users;
using Diploma.Bll.Services.Users.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Server.Controllers
{
    /// <summary>
    /// Контроллер управления пользователями
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("messenger/v{version:apiVersion}/users")]
    public sealed class UsersController : ControllerBase
    {
        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="userService">Сервис управления пользователями</param>
        /// <param name="username">Фильтрация по никнейму</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserResponse[]> GetUsersAsync(
            [FromServices] IUserService userService,
            [FromQuery] string username)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            var response = await userService.GetUsersAsync(username);

            return response;
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="userService">Сервис управления пользователями</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId:guid}")]
        public async Task<UserResponse> GetUserAsync(
            [FromServices] IUserService userService,
            Guid userId)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            var response = await userService.GetUserAsync(userId);

            return response;
        }
    }
}