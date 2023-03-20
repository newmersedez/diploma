using System.Numerics;
using Diploma.ECC.Math;
using Diploma.ECC.Math.Entities;

namespace Diploma.ECC.Encryption.Key
{
    /// <summary>
    /// Интерфейс класса генерации ключей
    /// </summary>
    public interface IKeyGenerator
    {
        /// <summary>
        /// Ключи шифрования
        /// </summary>
        KeyPair Keys { get; set; }

        /// <summary>
        /// Создать ключи шифрования
        /// </summary>
        /// <param name="curve">Эллиптическая кривая</param>
        /// <returns></returns>
        KeyPair GetKeyPair(Curve curve);

        /// <summary>
        /// Получить общий ключ
        /// </summary>
        /// <param name="privateKey">Приватный ключ</param>
        /// <param name="publicKey">Публичный ключ</param>
        /// <returns></returns>
        Point GetSharedSecret(BigInteger privateKey, Point publicKey);
    }
}