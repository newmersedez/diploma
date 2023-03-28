using System.Numerics;

namespace Diploma.Storage.Services.Signature.Request
{
    /// <summary>
    /// Запрос создания электронной подписи документа
    /// </summary>
    public sealed class SignFileRequest
    {
        /// <summary>
        /// Приватный ключ
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Папка
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string Name { get; set; }
    }
}