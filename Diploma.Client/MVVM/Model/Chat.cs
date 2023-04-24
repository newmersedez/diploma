using System;

namespace Diploma.Client.MVVM.Model
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public sealed class Chat
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Название чата
        /// </summary>
        public string Name { get; set; }
    }
}