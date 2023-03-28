using System.ComponentModel.DataAnnotations;

namespace Diploma.Storage.Services.Storage.Request
{
    /// <summary>
    /// Запрос на получение файла
    /// </summary>
    public sealed class DownloadFileRequest
    {
        /// <summary>
        /// Папка
        /// </summary>
        [Required]
        public string Folder { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Тип содержимого
        /// </summary>
        [Required]
        public string ContentType { get; set; }
    }
}