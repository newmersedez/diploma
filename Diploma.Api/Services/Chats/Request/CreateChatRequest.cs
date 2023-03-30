using System.Collections.Generic;
using Diploma.Persistence.Models.Enums;

namespace Diploma.Server.Services.Chats.Request
{
    /// <summary>
    /// Запрос на создение чата
    /// </summary>
    public sealed class CreateChatRequest
    {
        /// <summary>
        /// Название чата
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип чата
        /// </summary>
        public ChatType Type { get; set; }

        /// <summary>
        /// Участники чата
        /// </summary>
        public IEnumerable<ChatUserRequest> Users { get; set; }
    }
}