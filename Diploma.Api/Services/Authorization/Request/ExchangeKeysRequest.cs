using System;
using Diploma.Server.Services.Authorization.Response;

namespace Diploma.Server.Services.Authorization.Request
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