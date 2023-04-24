using System;

namespace Diploma.Client.MVVM.Model
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public sealed class Message
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
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Идентификатор вложения
        /// </summary>
        public Guid? AttachmentId { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}