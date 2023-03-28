using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Diploma.Storage.Services.Files;
using Diploma.Storage.Services.Files.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Storage.Controllers
{
    /// <summary>
    /// Контроллер управления файлами
    /// </summary>
    [ApiController]
    [Route("files")]
    public sealed class FilesController : ControllerBase
    {
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="fileService">Сервис управления файлами</param>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<UploadFileResponse> UploadFileAsync(
            [FromServices] IFileService fileService,
            [Required] IFormFile file)
        {
            if (fileService == null) throw new ArgumentNullException(nameof(fileService));

            var response = await fileService.UploadFileAsync(file);

            return response;
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="fileService">Сервис управления файлами</param>
        /// <param name="request">Запрос на скачивание файла</param>
        [HttpPost]
        [Route("download")]
        public async Task<FileStreamResult> DownloadFileAsync(
            [FromServices] IFileService fileService,
            DownloadFileRequest request)
        {
            var response = await fileService.DownloadFileAsync(request);

            return File(response.Content, request.ContentType, response.Name);
        }
    }
}