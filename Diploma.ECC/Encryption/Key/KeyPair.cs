using System.Numerics;
using Diploma.ECC.Math;
using Diploma.ECC.Math.Entities;

namespace Diploma.ECC.Encryption.Key
{
    /// <summary>
    /// Ключи шифрования
    /// </summary>
    public struct KeyPair
    {
        /// <summary>
        /// Приватный ключ
        /// </summary>
        public BigInteger PrivateKey;
        
        /// <summary>
        /// Публичный ключ
        /// </summary>
        public Point PublicKey;
    }
}