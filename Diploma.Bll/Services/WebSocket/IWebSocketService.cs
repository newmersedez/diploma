using System;
using System.Threading.Tasks;
using Diploma.Persistence.Models.Entities;

namespace Diploma.Bll.Services.WebSocket
{
    /// <summary>
    /// Интерфейс веб-сокет сервиса
    /// </summary>
    public interface IWebSocketService
    {
        /// <summary>
        /// Подписаться на события чатов
        /// </summary>
        /// <param name="webSocket">Веб-сокет</param>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task SubscribeChatWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid chatId);
        
        /// <summary>
        /// Подписаться на события пользователя
        /// </summary>
        /// <param name="webSocket">Веб-сокет</param>
        /// <param name="userId">Токен</param>
        /// <returns></returns>
        Task SubscribeUserWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid userId);

        /// <summary>
        /// Сообщение отправлено
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        Task NotifyMessageAddAsync(Guid chatId, Message message);

        /// <summary>
        /// Чат создан
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="token">Токен</param>
        /// <returns></returns>
        Task NotifyChatCreatedAsync(Guid chatId, Guid userId);
    }
}