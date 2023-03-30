using System;
using Diploma.Persistence.Models.Enums;
using Diploma.Server.Common.Responses;

namespace Diploma.Server.Services.Chat.Response
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