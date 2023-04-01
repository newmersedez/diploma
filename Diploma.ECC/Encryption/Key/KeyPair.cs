using System.Numerics;
using Diploma.Ecc.Math.Entities;

namespace Diploma.Ecc.Encryption.Key
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