using System;
using System.Security.Cryptography;

namespace Diploma.Storage.Common.FileHash
{
    /// <summary>
    /// Провайдер хэш-суммы файла
    /// </summary>
    public sealed class FileHashProvider : IFileHashProvider
    {
        /// <summary>
        /// Посчитать хэш-сумму
        /// </summary>
        /// <param name="content">Содержимое файла</param>
        /// <returns></returns>
        public string CalculateHashSum(byte[] content)
        {
            var md5 = new MD5CryptoServiceProvider();
            var checkSum = md5.ComputeHash(content);
            return BitConverter.ToString(checkSum).Replace("-", string.Empty);
        }
    }
}