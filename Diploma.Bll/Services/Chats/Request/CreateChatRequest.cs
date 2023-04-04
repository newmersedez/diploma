using System;
using System.Collections.Generic;

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
        /// Участники чата
        /// </summary>
        public List<Guid> Users { get; set; }
    }
}