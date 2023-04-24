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
        /// Хэш-сумма файла
        /// </summary>
        public string FileHash { get; set; }
    }
}