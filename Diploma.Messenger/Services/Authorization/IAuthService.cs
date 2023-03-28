using System.Threading.Tasks;
using Diploma.Server.Services.Authorization.Request;

namespace Diploma.Server.Services.Authorization
{
    /// <summary>
    /// Интерфейс сервиса авторизации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        Task RegisterUserAsync(UserAuthRequest request);
        
        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        Task LoginUserAsync(UserAuthRequest request);
    }
}