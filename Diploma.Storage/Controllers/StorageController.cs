using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Diploma.Storage.Services.Storage;
using Diploma.Storage.Services.Storage.Request;
using Diploma.Storage.Services.Storage.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Storage.Controllers
{
    /// <summary>
    /// Контроллер управления файлами
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("storage/v{version:apiVersion}/files")]
    public sealed class StorageController : ControllerBase
    {
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="storageService">Сервис управления файлами</param>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<UploadFileResponse> UploadFileAsync(
            [FromServices] IStorageService storageService,
            [Required] IFormFile file)
        {
            if (storageService == null) throw new ArgumentNullException(nameof(storageService));

            var response = await storageService.UploadFileAsync(file);

            return response;
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="storageService">Сервис управления файлами</param>
        /// <param name="request">Запрос на скачивание файла</param>
        [HttpPost]
        [Route("download")]
        public async Task<FileStreamResult> DownloadFileAsync(
            [FromServices] IStorageService storageService,
            DownloadFileRequest request)
        {
            var response = await storageService.DownloadFileAsync(request);

            return File(response.Content, request.ContentType, response.Name);
        }
    }
}