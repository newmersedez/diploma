using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Files;
using Diploma.Bll.Services.Files.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Server.Controllers
{
    /// <summary>
    /// Контроллер файлов
    /// </summary>
    [Authorize]
    [Route("files")]
    public class FilesController : ControllerBase
    {
        /// <summary>
        /// Создать файл
        /// </summary>
        /// <param name="fileService">Сервис управления файлами</param>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> CreateFileAsync(
            [FromServices] IFilesService fileService, CreateFileRequest request)
        {
            if (fileService == null) throw new ArgumentNullException(nameof(fileService));

            var response = await fileService.CreateFileAsync(request);

            return response;
        }
    }
}