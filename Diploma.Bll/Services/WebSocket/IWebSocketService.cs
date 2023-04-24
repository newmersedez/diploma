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
        Task SubscribeWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid chatId);

        /// <summary>
        /// Сообщение отправлено
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        Task NotifyMessageAddAsync(Guid chatId, Message message);
    }
}