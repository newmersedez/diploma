using Diploma.Bll.Common.Response;
using Diploma.Bll.Services.Authorization.Response;

namespace Diploma.Bll.Services.Authorization.Request
{
    /// <summary>
    /// Запрос с публичный ключом шифрования
    /// </summary>
    public sealed class ExchangeKeysRequest
    {
        /// <summary>
        /// Сгенерированный на стороне клиента публичный ключ
        /// </summary>
        public PublicKeyInfo PublicKey { get; set; }
    }
}