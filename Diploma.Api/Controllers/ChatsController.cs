using System;
using System.Threading.Tasks;
using Diploma.Server.Services.Chats;
using Diploma.Server.Services.Chats.Request;
using Diploma.Server.Services.Chats.Response;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Server.Controllers
{
    /// <summary>
    /// Контроллер управления чатами
    /// </summary>
    [ApiController]
    [Route("chats")]
    public sealed class ChatsController : ControllerBase
    {
        /// <summary>
        /// Получить список чатов
        /// </summary>
        /// <param name="chatService">Сервис управления чатами</param>
        [HttpGet]
        public async Task<ChatResponse[]> GetChatsAsync(
            [FromServices] IChatService chatService)
        {
            if (chatService == null) throw new ArgumentNullException(nameof(chatService));

            var response = await chatService.GetChatsAsync();

            return response;
        }


        /// <summary>
        /// Получить информацию о чате
        /// </summary>
        /// <param name="chatService">Сервис управления чатами</param>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{chatId:guid}")]
        public async Task<ChatResponse> GetChatAsync(
            [FromServices] IChatService chatService,
            Guid chatId)
        {
            if (chatService == null) throw new ArgumentNullException(nameof(chatService));

            var response = await chatService.GetChatAsync(chatId);

            return response;
        }
        
        /// <summary>
        /// Создать чат
        /// </summary>
        /// <param name="chatService">Сервис управления чатами</param>
        /// <param name="request">Запрос на создание чата</param>
        [HttpPost]
        public async Task<Guid> CreateChatAsync(
            [FromServices] IChatService chatService,
            CreateChatRequest request)
        {
            if (chatService == null) throw new ArgumentNullException(nameof(chatService));

            var response = await chatService.CreateChatAsync(request);

            return response;
        }
        
        /// <summary>
        /// Удалить чат
        /// </summary>
        /// <param name="chatService">Сервис управления чатами</param>
        /// <param name="chatId">Идентификатор чата</param>
        [HttpDelete]
        [Route("{chatId:guid}")]
        public async Task DeleteChatAsync(
            [FromServices] IChatService chatService,
            Guid chatId)
        {
            if (chatService == null) throw new ArgumentNullException(nameof(chatService));

            await chatService.DeleteChatAsync(chatId);
        }
    }
}