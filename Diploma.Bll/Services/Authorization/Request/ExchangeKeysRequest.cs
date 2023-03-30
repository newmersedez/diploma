using System;
using Diploma.Bll.Services.Authorization.Response;

namespace Diploma.Bll.Services.Authorization.Request
{
    /// <summary>
    /// Запрос с публичный ключом шифрования
    /// </summary>
    public sealed class ExchangeKeysRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Сгенерированный на стороне клиента публичный ключ
        /// </summary>
        public PublicKeyInfo PublicKey { get; set; }
    }
}