using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Messages;
using Diploma.Bll.Services.Messages.Request;
using Diploma.Bll.Services.Messages.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Server.Controllers
{
    /// <summary>
    /// Контроллер управления сообщениями
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("messenger/v{version:apiVersion}/chats/{chatId:guid}/messages")]
    public sealed class MessagesController : ControllerBase
    {
        /// <summary>
        /// Получить сообщения чата
        /// </summary>
        /// <param name="messageService">Сервис управления сообщениями</param>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageResponse[]> GetMessagesAsync(
            [FromServices] IMessageService messageService,
            Guid chatId)
        {
            if (messageService == null) throw new ArgumentNullException(nameof(messageService));

            var response = await messageService.GetMessagesAsync(chatId);

            return response;
        }

        /// <summary>
        /// Создать сообщение
        /// </summary>
        /// <param name="messageService">Сервис управления сообщениями</param>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="request">Запрос создания сообщения</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> CreateMessageAsync(
            [FromServices] IMessageService messageService,
            Guid chatId,
            CreateMessageRequest request)
        {
            if (messageService == null) throw new ArgumentNullException(nameof(messageService));

            var response = await messageService.CreateMessageAsync(chatId, request);

            return response;
        }
    }
}