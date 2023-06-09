using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Diploma.Bll.Services.Access;
using Diploma.Bll.Services.WebSocket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Server.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("messenger/v{version:apiVersion}/ws")]
    public sealed class WebSocketController : ControllerBase
    {
        private readonly IAccessManager _accessManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="accessManager">Сервис доступа</param>
        public WebSocketController(IAccessManager accessManager)
        {
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        /// <summary>
        /// Подписаться на веб сокет
        /// </summary>
        /// <param name="webSocketService">Сервис управления веб-сокетом</param>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="token">Токен</param>
        [HttpGet]
        [Route("chats/{chatId:guid}")]
        public async Task SubscribeChatWebSocketAsync(
            [FromServices] IWebSocketService webSocketService,
            Guid chatId,
            [Required] string token)
        {
            if (webSocketService == null) throw new ArgumentNullException(nameof(webSocketService));

            _accessManager.Token = token;

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await webSocketService.SubscribeChatWebSocketAsync(webSocket, chatId);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        
        /// <summary>
        /// Подписаться на веб сокет
        /// </summary>
        /// <param name="webSocketService">Сервис управления веб-сокетом</param>
        /// <param name="token">Токен</param>
        [HttpGet]
        [Route("user")]
        public async Task SubscribeUserWebSocketAsync(
            [FromServices] IWebSocketService webSocketService,
            [Required] string token)
        {
            if (webSocketService == null) throw new ArgumentNullException(nameof(webSocketService));
            
            _accessManager.Token = token;

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await webSocketService.SubscribeUserWebSocketAsync(webSocket, _accessManager.UserId);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}