using System;
using System.IO;
using System.Threading.Tasks;
using Diploma.Storage.Common.FileHash;
using Diploma.Storage.Common.PathBuilder;
using Diploma.Storage.Common.Verifier;
using Diploma.Storage.Services.Storage.Verify.Context;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Configuration;

namespace Diploma.Storage.Services.Storage
{
    /// <summary>
    /// Сервис файлового хранилища
    /// </summary>
    public class StorageService : Diploma.Storage.Storage.StorageBase
    {
        private readonly string STORAGE_ROOT;
        private readonly IFileHashProvider _fileHashProvider;
        private readonly IPathBuilder _pathBuilder;
        // private readonly Verifier<UploadFileContext> _uploadFileVerifier;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="pathBuilder">Строитель пути</param>
        /// <param name="fileHashProvider">Провайдер хэш-суммы</param>
        /// <param name="uploadFileVerifier">Верификация загрузки файлов</param>
        public StorageService(
            IConfiguration configuration,
            IPathBuilder pathBuilder, 
            IFileHashProvider fileHashProvider)
            // Verifier<UploadFileContext> uploadFileVerifier)
        {
            STORAGE_ROOT = configuration.GetValue<string>("Storage:Root");
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
            _fileHashProvider = fileHashProvider ?? throw new ArgumentNullException(nameof(fileHashProvider));
            // _uploadFileVerifier = uploadFileVerifier ?? throw new ArgumentNullException(nameof(uploadFileVerifier));
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="request">Запрос на загрузку файла</param>
        /// <param name="context">Контекст</param>
        /// <returns></returns>
        public override async Task<UploadFileResponse> UploadFileAsync(UploadFileRequest request, ServerCallContext context)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // await _uploadFileVerifier.VerifyAsync();

            var fileHashSum = _fileHashProvider.CalculateHashSum(request.Content.ToByteArray());
            var saveDirectory = _pathBuilder.Append(new[]
            {
                Directory.GetCurrentDirectory(),
                STORAGE_ROOT, 
                fileHashSum
            });

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            var filePath = _pathBuilder.Append(request.Name);
            await File.WriteAllBytesAsync(filePath, request.Content.ToByteArray());

            return new UploadFileResponse
            {
                Name = request.Name,
                Storage = fileHashSum,
                Status = Status.Success
            };
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="request">Запрос на скачивание файла</param>
        /// <param name="context">Контекст</param>
        /// <returns></returns>
        public override async Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request, ServerCallContext context)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: Верификация запроса
            
            var fileDirectory = _pathBuilder.Append(new[]
            {
                Directory.GetCurrentDirectory(), 
                STORAGE_ROOT, 
                request.Storage
            });

            if (!Directory.Exists(fileDirectory))
            {
                throw new ArgumentNullException(nameof(fileDirectory));
            }
            
            var filePath = _pathBuilder.Append(request.Name);
            var content = await File.ReadAllBytesAsync(filePath);

            return new DownloadFileResponse
            {
                Name = request.Name,
                Content = ByteString.CopyFrom(content)
            };
        }
    }
}