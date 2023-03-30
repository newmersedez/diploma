using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Extensions;

namespace Diploma.Bll.Common.Providers.KeysProvider
{
    /// <summary>
    /// Провайдер ключей шифрования
    /// </summary>
    public sealed class KeysProvider : IKeysProvider
    {
        /// <summary>
        /// Создать пару ключей
        /// </summary>
        /// <param name="curve">Эллиптическая кривая</param>
        /// <returns></returns>
        public KeyPair CreateKeyPair(Curve curve)
        {
            var result = new KeyPair();
            var keyBytes = curve.Parameters.Length / 8;

            var unsignedBytes = new byte[] { 0x00 };
            var randomBytes = new byte[keyBytes];

            var randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(randomBytes);

            var positiveRandomBytes = randomBytes.Concat(unsignedBytes).ToArray();
            var randomValue = new BigInteger(positiveRandomBytes);

            do
            {
                result.PrivateKey = (randomValue % curve.Parameters.N);
            }
            while (result.PrivateKey == 0);


            result.PublicKey = curve.Parameters.G.Multiply(result.PrivateKey);
            return result;
        }
    }
}