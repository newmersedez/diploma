namespace Diploma.Bll.Services.WebSocket
{
    /// <summary>
    /// Типы уведомлений
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Добавлено сообщение в чате
        /// </summary>
        MESSAGE_ADDED = 0,
        
        /// <summary>
        /// Создан новый чат
        /// </summary>
        CHAT_CREATED = 1
    }
}