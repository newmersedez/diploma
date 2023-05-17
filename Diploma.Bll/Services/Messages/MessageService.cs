using System;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Bll.Services.Access;
using Diploma.Bll.Services.Messages.Request;
using Diploma.Bll.Services.Messages.Response;
using Diploma.Bll.Services.WebSocket;
using Diploma.Persistence;
using Diploma.Persistence.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Bll.Services.Messages
{
    /// <summary>
    /// Сервис управления сообщениями
    /// </summary>
    public sealed class MessageService : IMessageService
    {
        private readonly DatabaseContext _context;
        private readonly IAccessManager _accessManager;
        private readonly IWebSocketService _webSocketService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="webSocketService">Сервис управления веб-сокетом</param>
        /// <param name="accessManager">Сервис доступа</param>
        public MessageService(DatabaseContext context, IWebSocketService webSocketService, IAccessManager accessManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _webSocketService = webSocketService ?? throw new ArgumentNullException(nameof(webSocketService));
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        /// <summary>
        /// Получить сообщения
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        public async Task<MessageResponse[]> GetMessagesAsync(Guid chatId)
        {
            // TODO: Верификация сущуствования чата

            return await _context.Messages
                .Where(x => x.ChatId == chatId)
                .Select(x => new MessageResponse
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Username = x.User.Username,
                    AttachmentId = x.AttachmentId,
                    AttachmentName = x.Attachment.Name,
                    Text = x.Text,
                    DateCreate = x.DateCreate
                })
                .OrderBy(x => x.DateCreate)
                .ToArrayAsync();
        }

        /// <summary>
        /// Создать сообщение
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="request">Запрос создания сообщения</param>
        /// <returns></returns>
        public async Task<Guid> CreateMessageAsync(Guid chatId, CreateMessageRequest request)
        {
            // TODO: Верификация чата, запроса, доступа

            var message = new Message
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                UserId = _accessManager.UserId,
                AttachmentId = request.AttachmentId,
                Text = request.Text,
                DateCreate = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await _webSocketService.NotifyMessageAddAsync(chatId, message);

            return message.Id;
        }
    }
}