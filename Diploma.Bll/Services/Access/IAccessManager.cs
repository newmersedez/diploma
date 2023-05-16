using System;

namespace Diploma.Bll.Services.Access
{
    /// <summary>
    /// Менеджер доступа
    /// </summary>
    public interface IAccessManager
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }
}