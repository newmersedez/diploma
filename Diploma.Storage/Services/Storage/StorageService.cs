using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;

namespace Diploma.Storage.Services.Storage
{
    /// <summary>
    /// Сервис файлового хранилища
    /// </summary>
    public class StorageService : Diploma.Storage.Storage.StorageBase
    {
        private readonly string STORAGE_ROOT;
        
        public StorageService(IConfiguration configuration)
        {
            STORAGE_ROOT = configuration.GetValue<string>("Storage:Root");
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="request">Запрос на загрузку файла</param>
        /// <param name="context">Контекст</param>
        /// <returns></returns>
        public override Task<UploadFileResponse> UploadFileAsync(UploadFileRequest request, ServerCallContext context)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var currentDirectory = Directory.GetCurrentDirectory();
            var storageDirectory = Path.Combine(currentDirectory, STORAGE_ROOT);
            var fileDirectory = Path.Combine(storageDirectory, request.Metadata.Name);

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            var filePath = Path.Combine(fileDirectory, request.Metadata.Name);

            var bytes = request.File.Content.ToByteArray();
            System.IO.File.WriteAllBytes(filePath, bytes);

            return Task.FromResult(new UploadFileResponse
            {
                Name = filePath,
                Status = Status.Success
            });
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="request">Запрос на скачивание файла</param>
        /// <param name="context">Контекст</param>
        /// <returns></returns>
        public override Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request, ServerCallContext context)
        {
            return base.DownloadFileAsync(request, context);
        }
    }
}