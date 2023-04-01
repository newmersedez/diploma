using System;

namespace Diploma.Bll.Services.Authorization.Response
{
    /// <summary>
    /// Ответ при авторизации
    /// </summary>
    public sealed class UserAuthResponse
    {
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }
}