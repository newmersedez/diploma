using System;
using System.Collections.Generic;

namespace Diploma.Server.Services.Chat.Response
{
    /// <summary>
    /// Ответ при получении информации о чате
    /// </summary>
    public sealed class ChatResponse
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название чата
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Пользователи чата
        /// </summary>
        public List<ChatUserResponse> Users { get; set; }
    }
}