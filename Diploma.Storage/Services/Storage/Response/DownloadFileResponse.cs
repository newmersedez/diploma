using System.IO;

namespace Diploma.Storage.Services.Storage.Response
{
    /// <summary>
    /// Ответ при скачивании файла
    /// </summary>
    public sealed class DownloadFileResponse
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип содержимого
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        public Stream Content { get; set; }
    }
}