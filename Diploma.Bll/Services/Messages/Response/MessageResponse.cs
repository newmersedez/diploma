using System;

namespace Diploma.Bll.Services.Messages.Response
{
    /// <summary>
    /// Информация о сообщении
    /// </summary>
    public sealed class MessageResponse
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Никнейм пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Идентификатор вложения
        /// </summary>
        public Guid? AttachmentId { get; set; }

        /// <summary>
        /// Идентификатор текста
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата написания сообщения
        /// </summary>
        public DateTime DateCreate { get; set; }
    }
}