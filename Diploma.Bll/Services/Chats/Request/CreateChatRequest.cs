using System.Collections.Generic;
using Diploma.Persistence.Models.Enums;

namespace Diploma.Bll.Services.Chats.Request
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
        public List<ChatUserRequest> Users { get; set; }
    }
}