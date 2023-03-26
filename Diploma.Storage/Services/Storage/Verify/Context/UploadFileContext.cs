namespace Diploma.Storage.Services.Storage.Verify.Context
{
    /// <summary>
    /// Контекст загрузки файлоа
    /// </summary>
    public sealed class UploadFileContext
    {
        /// <summary>
        /// Запрос на загрузку файла
        /// </summary>
        public UploadFileRequest Request { get; set; }
    }
}