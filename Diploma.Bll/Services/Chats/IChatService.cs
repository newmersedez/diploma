using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Chats.Request;
using Diploma.Bll.Services.Chats.Response;

namespace Diploma.Bll.Services.Chats
{
    /// <summary>
    /// Интерфейс сервиса чата
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Получить список чатов
        /// </summary>
        /// <returns></returns>
        Task<ChatResponse[]> GetChatsAsync();

        /// <summary>
        /// Получить информацию о чате
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task<ChatResponse> GetChatAsync(Guid chatId);

        /// <summary>
        /// Создать чат
        /// </summary>
        /// <param name="request">Запрос на создание чата</param>
        /// <returns></returns>
        Task<Guid> CreateChatAsync(CreateChatRequest request);

        /// <summary>
        /// Удалить чат
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task DeleteChatAsync(Guid chatId);
    }
}