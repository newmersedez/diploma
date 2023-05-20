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
        public Guid? FileId { get; set; }

        /// <summary>
        /// Название вложения
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Электронная подпись файла
        /// </summary>
        public string Signature { get; set; }

        public bool IsSigned => string.IsNullOrEmpty(Signature);

        public string IsSignedAsText => FileId.HasValue ? "Подписан ✔" : string.Empty;
        
        public override string ToString()
        {
            return Text;
        }
    }
}