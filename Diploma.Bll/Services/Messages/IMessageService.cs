using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Messages.Request;
using Diploma.Bll.Services.Messages.Response;

namespace Diploma.Bll.Services.Messages
{
    /// <summary>
    /// Интерфейс сервиса управления сообщениями
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Получить сообщения чата 
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task<MessageResponse[]> GetMessagesAsync(Guid chatId);
        
        /// <summary>
        /// Создать сообщение
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="request">Запрос создания сообщения</param>
        /// <returns></returns>
        Task<Guid> CreateMessageAsync(Guid chatId, CreateMessageRequest request);
    }
}