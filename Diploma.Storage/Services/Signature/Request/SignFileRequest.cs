using System.Numerics;
using Diploma.Storage.Common.Responses;

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
        /// Информация о файле
        /// </summary>
        public File File { get; set; }
    }
}