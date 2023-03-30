using System;
using Diploma.Persistence.Models.Enums;

namespace Diploma.Server.Services.Chats.Request
{
    /// <summary>
    /// Запрос создания пользователя чата
    /// </summary>
    public sealed class ChatUserRequest
    {
        /// <summary>
        /// Участник чата
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public ChatRole Role { get; set; }
    }
}