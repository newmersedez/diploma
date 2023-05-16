using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Bll.Common.Response;
using Diploma.Bll.Services.Access;
using Diploma.Bll.Services.Authorization.Response;
using Diploma.Bll.Services.Chats.Request;
using Diploma.Bll.Services.Chats.Response;
using Diploma.Bll.Services.WebSocket;
using Diploma.Persistence;
using Diploma.Persistence.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Bll.Services.Chats
{
    /// <summary>
    /// Сервис управления чатами
    /// </summary>
    public sealed class ChatService : IChatService
    {
        private readonly DatabaseContext _context;
        private readonly IAccessManager _accessManager;
        private readonly IWebSocketService _webSocketService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Конекст БД</param>
        /// <param name="accessManager">Сервис управления доступом</param>
        /// <param name="webSocketService"></param>
        public ChatService(DatabaseContext context, IAccessManager accessManager, IWebSocketService webSocketService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
            _webSocketService = webSocketService ?? throw new ArgumentNullException(nameof(webSocketService));
        }

        /// <summary>
        /// Получить список чатов
        /// </summary>
        /// <returns></returns>
        public async Task<ChatResponse[]> GetChatsAsync()
        {
            return await _context.Chats
                .Where(x => x.ChatUsers.Any(y => y.UserId == _accessManager.UserId))
                .Select(x => new ChatResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Users = x.ChatUsers
                        .Select(y => new ChatUserResponse
                        {
                            Id = y.Id,
                            Email = y.User.Email,
                            PublicKey = new PublicKeyInfo
                            {
                                X = y.User.PublicKey.X,
                                Y = y.User.PublicKey.Y
                            }
                        })
                        .ToList()
                })
                .ToArrayAsync();
        }

        /// <summary>
        /// Получить информацию о чате
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        public async Task<ChatResponse> GetChatAsync(Guid chatId)
        {
            // TODO: Верификация что чат существует

            return await _context.Chats
                .Where(x => x.Id == chatId)
                .Select(x => new ChatResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Users = x.ChatUsers
                        .Select(y => new ChatUserResponse
                        {
                            Id = y.Id,
                            Email = y.User.Email,
                            PublicKey = new PublicKeyInfo
                            {
                                X = y.User.PublicKey.X,
                                Y = y.User.PublicKey.Y
                            }
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Создать чат
        /// </summary>
        /// <param name="request">Запрос на создание чата</param>
        /// <returns></returns>
        public async Task<Guid> CreateChatAsync(CreateChatRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: Верификация запроса

            var chat = new Chat
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
            };

            _context.Chats.Add(chat);

            var users = new List<Guid> { request.UserId, _accessManager.UserId };
            
            foreach (var userId in users)
            {
                var chatUser = new ChatUser
                {
                    Id = Guid.NewGuid(),
                    ChatId = chat.Id,
                    UserId = userId,
                };
                _context.ChatUser.Add(chatUser);
            }

            await _context.SaveChangesAsync();
            
            await _webSocketService.NotifyChatCreatedAsync(chat.Id, request.UserId);
            
            return chat.Id;
        }

        /// <summary>
        /// Удалить чат
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        public async Task DeleteChatAsync(Guid chatId)
        {
            // TODO: Верификация существования чата

            await _context.Chats
                .Where(x => x.Id == chatId)
                .DeleteFromQueryAsync();
            
            await _context.SaveChangesAsync();
        }
    }
}