using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Diploma.ECC.Math;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Extensions;

namespace Diploma.ECC.Encryption.Key
{
    /// <summary>
    /// Генератор ключей шифрования
    /// </summary>
    public class KeysGenerator : IKeyGenerator
    {
        /// <summary>
        /// Ключ шифрования
        /// </summary>
        public KeyPair Keys { get; set; }

        public KeyPair GetKeyPair(Curve curve)
        {
            KeyPair result = new KeyPair();
            uint keyBytes = curve.Parameters.Length / 8;

            byte[] unsignedBytes = new byte[] { 0x00 };
            byte[] randomBytes = new byte[keyBytes];

            var randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(randomBytes);

            byte[] positiveRandomBytes = randomBytes.Concat(unsignedBytes).ToArray();
            BigInteger randomValue = new BigInteger(positiveRandomBytes);

            do
            {
                result.PrivateKey = (randomValue % curve.Parameters.N);
            }
            while (result.PrivateKey == 0);


            result.PublicKey = curve.Parameters.G.Multiply(result.PrivateKey);
            return result;
        }

        /// <summary>
        /// Получить общий ключ
        /// </summary>
        /// <param name="privateKey">Приватный ключ</param>
        /// <param name="publicKey">Публичный ключ</param>
        /// <returns></returns>
        public Point GetSharedSecret(BigInteger privateKey, Point publicKey) => publicKey.Multiply(privateKey);
    }
}