using System;

namespace Diploma.Client.MVVM.Model
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }
    }
}