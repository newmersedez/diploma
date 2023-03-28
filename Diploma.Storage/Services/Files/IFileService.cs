using System.Threading.Tasks;
using Diploma.Storage.Services.Files.Request;
using Microsoft.AspNetCore.Http;

namespace Diploma.Storage.Services.Files
{
    /// <summary>
    /// Интерфейс сервиса управления файлами
    /// </summary>
    public interface IFileService
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