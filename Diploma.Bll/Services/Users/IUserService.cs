using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Users.Response;

namespace Diploma.Bll.Services.Users
{
    /// <summary>
    /// Интерфейс сервиса управления пользователями
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <param name="username">Фильтрация по никнейму</param>
        /// <returns></returns>
        Task<UserResponse[]> GetUsersAsync(string username);

        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<UserResponse> GetUserAsync(Guid userId);
    }
}