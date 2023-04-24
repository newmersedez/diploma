using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Diploma.Bll.Common.Exceptions;
using Diploma.Persistence;
using Diploma.Persistence.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Bll.Services.WebSocket
{
    /// <summary>
    /// Сервис управления веб-сокетом
    /// </summary>
    public sealed class WebSocketService : IWebSocketService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConcurrentDictionary<Guid, LockedList<System.Net.WebSockets.WebSocket>> _subscribers;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceScopeFactory">Scoped фабрика</param>
        public WebSocketService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _subscribers = new ConcurrentDictionary<Guid, LockedList<System.Net.WebSockets.WebSocket>>();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                },
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        /// <summary>
        /// Подписаться на события чатов
        /// </summary>
        /// <param name="webSocket">Веб-сокет</param>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        public async Task SubscribeWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid chatId)
        {
            if (webSocket == null) throw new ArgumentNullException(nameof(webSocket));

            if (!_subscribers.ContainsKey(chatId))
            {
                _subscribers[chatId] = new LockedList<System.Net.WebSockets.WebSocket>();
            }

            _subscribers[chatId].Add(webSocket);

            var buffer = new byte[1024 * 4];

            if (webSocket.State == WebSocketState.Closed)
            {
                throw new RequestException(HttpStatusCode.BadRequest,
                    "Поток закрыт для отображения сообщений, Повторите попытку");
            }

            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            
            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);

            if (_subscribers.TryGetValue(chatId, out var value))
            {
                value.Remove(webSocket);
                if (value.Count == 0)
                {
                    _subscribers.Remove(chatId, out _);
                }
            }
        }

        public async Task NotifyMessageAddAsync(Guid chatId, Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var messageInfo = await context.Messages
                .Where(x => x.Id == message.Id)
                .Select(x => new
                {
                    Message = x,
                    x.User,
                    x.Attachment
                })
                .FirstOrDefaultAsync();

            var notification = new Notification
            {
                Type = NotificationType.MESSAGE_ADDED,
                Payload = new
                {
                    id = messageInfo.Message.Id,
                    email = messageInfo.User?.Email,
                    attachmentId = messageInfo.Attachment?.Id,
                    attachmentName = messageInfo.Attachment?.Name,
                    date = message.DateCreate
                }
            };

            var test = JsonSerializer.Serialize(notification, _jsonSerializerOptions);

            var buffer = Encoding.UTF8.GetBytes(test);

            if (_subscribers.TryGetValue(chatId, out var sockets))
            {
                foreach (var webSocket in sockets)
                {
                    try
                    {
                        await webSocket.SendAsync(
                            new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch (Exception)
                    {
                        sockets.Remove(webSocket);
                    }
                }
            }
        }
    }
}