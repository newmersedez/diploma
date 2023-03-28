using System.ComponentModel.DataAnnotations;

namespace Diploma.Storage.Services.Files
{
    /// <summary>
    /// Ответ при загрузке файла
    /// </summary>
    public sealed class UploadFileResponse
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