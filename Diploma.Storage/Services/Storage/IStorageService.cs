using System.Threading.Tasks;
using Diploma.Storage.Services.Storage.Request;
using Diploma.Storage.Services.Storage.Response;
using Microsoft.AspNetCore.Http;

namespace Diploma.Storage.Services.Storage
{
    /// <summary>
    /// Интерфейс сервиса управления файлами
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        Task<UploadFileResponse> UploadFileAsync(IFormFile file);

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="request">Запрос на скачивание файла</param>
        /// <returns></returns>
        Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request);
    }
}