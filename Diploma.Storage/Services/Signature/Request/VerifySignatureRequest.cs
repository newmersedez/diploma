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
        public PublicKeyInfo PublicKey { get; set; }

        /// <summary>
        /// Электронная подпись
        /// </summary>
        public SignatureInfo Signature { get; set; }

        /// <summary>
        /// Хэш-сумма файла
        /// </summary>
        public string FileHash { get; set; }
    }
}