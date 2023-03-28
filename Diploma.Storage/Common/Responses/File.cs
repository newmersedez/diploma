namespace Diploma.Storage.Common.Responses
{
    /// <summary>
    /// Сущность файла
    /// </summary>
    public sealed class File
    {
        /// <summary>
        /// Папка файла
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string Name { get; set; }
    }
}