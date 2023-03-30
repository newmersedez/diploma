using System;
using Diploma.Server.Services.Authorization.Response;

namespace Diploma.Server.Services.Chats.Response
{
    /// <summary>
    /// Ответ при получении информации о пользователе чата
    /// </summary>
    public sealed class ChatUserResponse
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Электронный адрес пользователя
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Публичный ключ пользователя
        /// </summary>
        public PublicKeyInfo PublicKey { get; set; }
    }
}