using System.Numerics;
using Diploma.Ecc.Math;
using Diploma.Ecc.Math.Entities;

namespace Diploma.Ecc.Encryption.Key
{
    /// <summary>
    /// Интерфейс класса генерации ключей
    /// </summary>
    public interface IKeyGenerator
    {
        /// <summary>
        /// Создать ключи шифрования
        /// </summary>
        /// <param name="curve">Эллиптическая кривая</param>
        /// <returns></returns>
        KeyPair CreateKeyPair(Curve curve);

        /// <summary>
        /// Получить общий ключ
        /// </summary>
        /// <param name="privateKey">Приватный ключ</param>
        /// <param name="publicKey">Публичный ключ</param>
        /// <returns></returns>
        Point GetSharedSecret(BigInteger privateKey, Point publicKey);
    }
}