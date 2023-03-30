using System;

namespace Diploma.Server.Services.Access
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
    }
}