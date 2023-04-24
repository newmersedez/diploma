namespace Diploma.Bll.Services.WebSocket
{
    /// <summary>
    /// Уведомление
    /// </summary>
    public sealed class Notification
    {
        /// <summary>
        /// Тип уведомления
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Содержимое уведомления
        /// </summary>
        public object Payload { get; set; }
    }
}