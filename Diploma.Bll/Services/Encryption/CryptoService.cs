using System;
using System.Security.Cryptography;
using System.Text;

namespace Diploma.Bll.Services.Encryption
{
    /// <summary>
    /// Сервис шифрования алгоритмом AEC
    /// 256 bit key, ECB
    /// </summary>
    public sealed class CryptoService : ICryptoService
    {
        /// <summary>
        /// Зашифровать сообщение
        /// </summary>
        /// <param name="plainText">Текст</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns>base64 string</returns>
        public string Encrypt(string plainText, byte[] key)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));

            var aesProvider = Aes.Create();
            aesProvider.Key = key;
            aesProvider.Mode = CipherMode.ECB;

            var encryptor = aesProvider.CreateEncryptor();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedTextBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            return Convert.ToBase64String(encryptedTextBytes);
        }

        /// <summary>
        /// Дешифровать сообщение
        /// </summary>
        /// <param name="encryptedText">Шифротекст</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns>base64 string</returns>
        public string Decrypt(string encryptedText, byte[] key)
        {
            if (encryptedText == null) throw new ArgumentNullException(nameof(encryptedText));
            
            var aesProvider = Aes.Create();
            aesProvider.Key = key;
            aesProvider.Mode = CipherMode.ECB;
            
            var decryptor = aesProvider.CreateDecryptor();
            var encryptedTextBytes = Convert.FromBase64String(encryptedText);
            var plainTextBytes = decryptor.TransformFinalBlock(encryptedTextBytes, 0, encryptedTextBytes.Length);

            return Encoding.UTF8.GetString(plainTextBytes);
        }
    }
}