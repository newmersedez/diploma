using System.Threading.Tasks;
using Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Request.Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Response;

namespace Diploma.Bll.Services.Authorization
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
        Task<UserAuthResponse> RegisterUserAsync(RegisterUserRequest request);
        
        /// <summary>
        /// Залогинить пользователя
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns></returns>
        Task<UserAuthResponse> LoginUserAsync(LoginUserRequest request);
    }
}