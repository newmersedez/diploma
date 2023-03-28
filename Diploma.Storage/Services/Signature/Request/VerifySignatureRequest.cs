using Diploma.Storage.Common.Responses;

namespace Diploma.Storage.Services.Signature.Request
{
    /// <summary>
    /// Запрос на проверку электронной подписи
    /// </summary>
    public sealed class VerifySignatureRequest
    {
        /// <summary>
        /// Публичный ключ
        /// </summary>
        public PublicKey PublicKey { get; set; }

        /// <summary>
        /// Электронная подпись
        /// </summary>
        public Common.Responses.Signature Signature { get; set; }

        /// <summary>
        /// Информация о файле
        /// </summary>
        public File File { get; set; }
    }
}