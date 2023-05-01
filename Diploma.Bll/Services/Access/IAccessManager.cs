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
        public Guid UserId => Guid.Parse("f29e4524-73a7-4491-9bf1-19f72538e52d");
        
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }
}