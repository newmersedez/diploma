using System;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Persistence;
using Diploma.Server.Common.Responses;
using Diploma.Server.Services.AccessManager;
using Diploma.Server.Services.Chat.Response;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Server.Services.Chat
{
    /// <summary>
    /// Сервис управления чатами
    /// </summary>
    public sealed class ChatService : IChatService
    {
        private readonly DatabaseContext _context;
        private readonly IAccessManager _accessManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Конекст БД</param>
        /// <param name="accessManager">Сервис управления доступом</param>
        public ChatService(DatabaseContext context, IAccessManager accessManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        /// <summary>
        /// Получить список чатов
        /// </summary>
        /// <returns></returns>
        public async Task<ChatResponse[]> GetChatsAsync()
        {
            return await _context.Chats
                .Where(x => x.ChatUsers.Select(y => y.UserId).Contains(_accessManager.UserId))
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
    }
}