using System.Threading.Tasks;
using Diploma.Server.Services.Authorization.Request;
using Diploma.Server.Services.Authorization.Response;

namespace Diploma.Server.Services.Authorization
{
    /// <summary>
    /// Интерфейс сервиса авторизации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Обменяться ключами шифрования
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        Task<ExchangeKeysResponse> ExchangeKeysAsync(ExchangeKeysRequest request);
        
        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        Task<UserAuthResponse> RegisterUserAsync(UserAuthRequest request);
        
        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        Task<UserAuthResponse> LoginUserAsync(UserAuthRequest request);
    }
}