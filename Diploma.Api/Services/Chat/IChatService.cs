using System;
using System.Threading.Tasks;
using Diploma.Server.Services.Chat.Response;

namespace Diploma.Server.Services.Chat
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
    }
}