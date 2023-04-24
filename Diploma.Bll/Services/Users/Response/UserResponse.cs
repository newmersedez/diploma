using System;
using Diploma.Bll.Common.Response;
using Diploma.Bll.Services.Authorization.Response;

namespace Diploma.Bll.Services.Users.Response
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public sealed class UserResponse
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Публичный ключ пользователя
        /// </summary>
        public PublicKeyInfo PublicKey { get; set; }
    }
}