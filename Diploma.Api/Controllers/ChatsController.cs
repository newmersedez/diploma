using System;
using System.Threading.Tasks;
using Diploma.Server.Services.Chat;
using Diploma.Server.Services.Chat.Response;
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
        
        [HttpPost]
        public async Task CreateChatAsync()
        {
            throw new NotImplementedException();    
        }
        
        [HttpDelete]
        [Route("{chatId:guid}")]
        public async Task DeleteChatAsync()
        {
            throw new NotImplementedException();    
        }
    }
}