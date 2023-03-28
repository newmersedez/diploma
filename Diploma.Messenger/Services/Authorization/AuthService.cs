using System.Threading.Tasks;
using Diploma.Server.Services.Authorization.Request;

namespace Diploma.Server.Services.Authorization
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="authRequest">Объект запроса</param>
        /// <returns></returns>
        public Task RegisterUserAsync(UserAuthRequest authRequest)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        public Task LoginUserAsync(UserAuthRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}