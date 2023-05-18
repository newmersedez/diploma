using System;

namespace Diploma.Bll.Services.Messages.Request
{
    /// <summary>
    /// Запрос создания сообщения
    /// </summary>
    public sealed class CreateMessageRequest
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public Guid? FileId { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }
    }
}