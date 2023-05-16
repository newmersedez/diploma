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
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Bll.Services.WebSocket
{
    /// <summary>
    /// Сервис управления веб-сокетом
    /// </summary>
    public sealed class WebSocketService : IWebSocketService
    {
        private readonly ConcurrentDictionary<Guid, LockedList<System.Net.WebSockets.WebSocket>> _chatSubscribers;
        private readonly ConcurrentDictionary<Guid, System.Net.WebSockets.WebSocket> _usersHub;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceScopeFactory">Scoped фабрика</param>
        public WebSocketService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _chatSubscribers = new ConcurrentDictionary<Guid, LockedList<System.Net.WebSockets.WebSocket>>();
            _usersHub = new ConcurrentDictionary<Guid, System.Net.WebSockets.WebSocket>();
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
        public async Task SubscribeChatWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid chatId)
        {
            if (webSocket == null) throw new ArgumentNullException(nameof(webSocket));

            if (!_chatSubscribers.ContainsKey(chatId))
            {
                _chatSubscribers[chatId] = new LockedList<System.Net.WebSockets.WebSocket>();
            }

            _chatSubscribers[chatId].Add(webSocket);

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

            if (_chatSubscribers.TryGetValue(chatId, out var value))
            {
                value.Remove(webSocket);
                if (value.Count == 0)
                {
                    _chatSubscribers.Remove(chatId, out _);
                }
            }
        }

        /// <summary>
        /// Подписаться на события чатов
        /// </summary>
        /// <param name="webSocket">Веб-сокет</param>
        /// <param name="userId">Токен</param>
        /// <returns></returns>
        public async Task SubscribeUserWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid userId)
        {
            if (webSocket == null) throw new ArgumentNullException(nameof(webSocket));

            if (!_usersHub.ContainsKey(userId))
            {
                _usersHub[userId] = webSocket;
            }
            
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

            if (_usersHub.TryGetValue(userId, out var _))
            {
                _chatSubscribers.Remove(userId, out _);
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

            if (_chatSubscribers.TryGetValue(chatId, out var sockets))
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

        public async Task NotifyChatCreatedAsync(Guid chatId, Guid userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var chatInfo = await context.Chats
                .Where(x => x.Id == chatId)
                .Select(x => new
                {
                    Chat = x,
                    Users = x.ChatUsers.Select(y => new
                    {
                        y.User,
                        y.User.PublicKey
                    })
                })
                .FirstOrDefaultAsync();

            var notification = new Notification
            {
                Type = NotificationType.CHAT_CREATED,
                Payload = new
                {
                    id = chatInfo.Chat.Id,
                    name = chatInfo.Chat.Name,
                    users = chatInfo.Users
                        .Select(x => new
                        {
                            id = x.User.Id,
                            name = x.User.Username,
                            email = x.User.Email,
                            publicKey = new
                            {
                                y = x.PublicKey.X,
                                x = x.PublicKey.Y,
                            }
                        })
                }
            };

            var test = JsonSerializer.Serialize(notification, _jsonSerializerOptions);

            var buffer = Encoding.UTF8.GetBytes(test);

            if (_usersHub.TryGetValue(userId, out var webSocket))
            {
                try
                {
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception)
                {
                    _usersHub.Remove(userId, out _);
                }
            }
        }
    }
}