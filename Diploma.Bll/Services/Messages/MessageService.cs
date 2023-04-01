using System;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Bll.Services.Messages.Request;
using Diploma.Bll.Services.Messages.Response;
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

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public MessageService(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
                .Where(x => x.Id == chatId)
                .Select(x => new MessageResponse
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    AttachmentId = x.AttachmentId,
                    Text = x.Text,
                    DateCreate = x.DateCreate
                })
                .OrderByDescending(x => x.DateCreate)
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
                UserId = request.UserId,
                AttachmentId = request.AttachmentId,
                Text = request.Text,
                DateCreate = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message.Id;
        }
    }
}