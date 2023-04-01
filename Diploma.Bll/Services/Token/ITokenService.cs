using System;

namespace Diploma.Bll.Services.Token
{
    /// <summary>
    /// Интерфейс сервиса управления токеном
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Генерация токена пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        string Generate(Guid userId);

        /// <summary>
        /// Получить данные из токена
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns></returns>
        Guid GetUserId(string token);
    }
}