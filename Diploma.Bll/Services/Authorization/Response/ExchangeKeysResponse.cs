using Diploma.Bll.Common.Responses;

namespace Diploma.Bll.Services.Authorization.Response
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