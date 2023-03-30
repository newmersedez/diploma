namespace Diploma.Server.Services.Encryption
{
    /// <summary>
    /// Интерфейс сервиса шифрования
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Зашифровать сообшение
        /// </summary>
        /// <param name="plainText">Текст</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns></returns>
        string Encrypt(string plainText, byte[] key);

        /// <summary>
        /// Дешифровать сообщеие
        /// </summary>
        /// <param name="encryptedText">Шифротекст</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns></returns>
        string Decrypt(string encryptedText, byte[] key);
    }
}