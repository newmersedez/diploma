using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Diploma.Storage.Common.Exceptions;
using Diploma.Storage.Common.Providers.FileHash;
using Diploma.Storage.Services.Storage.Request;
using Diploma.Storage.Services.Storage.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Diploma.Storage.Services.Storage
{
    /// <summary>
    /// Сервис управления файлами
    /// </summary>
    public sealed class StorageService : IStorageService
    {
        private readonly string STORAGE_ROOT;
        private readonly IFileHashProvider _fileHashProvider;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="fileHashProvider">Провайдер хэш-суммы файла</param>
        public StorageService(
            IConfiguration configuration,
            IFileHashProvider fileHashProvider)
        {
            STORAGE_ROOT = configuration.GetValue<string>("Storage:Root");
            _fileHashProvider = fileHashProvider ?? throw new ArgumentNullException(nameof(fileHashProvider));
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        public async Task<UploadFileResponse> UploadFileAsync(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            
            // TODO: верификация параметров
            // TODO: все связанное с путем в filePathProvider

            byte[] content;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                content = stream.ToArray();
            }
            
            var folderName = _fileHashProvider.CalculateHashSum(content);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), STORAGE_ROOT, folderName);
            
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);
            await File.WriteAllBytesAsync(filePath, content);

            return new UploadFileResponse
            {
                Folder = folderName,
                Name = file.FileName,
                ContentType = file.ContentType
            };
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="request">Запрос на скачивание файла</param>
        /// <returns></returns>
        public async Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: верификация параметров

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), STORAGE_ROOT, request.Folder, request.Name);

            if (!File.Exists(filePath))
            {
                throw new RequestException(HttpStatusCode.NotFound, "Файл по указанному пути не существует");
            }

            var content = await File.ReadAllBytesAsync(filePath);

            return new DownloadFileResponse
            {
                Name = request.Name,
                ContentType = request.ContentType,
                Content = new MemoryStream(content)
            };
        }
    }
}