using Diploma.Server.Common.Responses;

namespace Diploma.Server.Services.Authorization
{
    /// <summary>
    /// Ответ при обмене ключами
    /// </summary>
    public class ExchangeKeysResponse
    {
        /// <summary>
        /// Публичный ключ сервера
        /// </summary>
        public PublicKeyInfo ServerPublicKey { get; set; }

        /// <summary>
        /// Зашифрованный приватный ключ пользователя
        /// </summary>
        public string UserPrivateKey { get; set; }
    }
}