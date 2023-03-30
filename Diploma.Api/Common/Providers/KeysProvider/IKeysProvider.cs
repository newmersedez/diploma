using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;

namespace Diploma.Server.Services.KeysProvider
{
    /// <summary>
    /// Провайдер ключей шифрования
    /// </summary>
    public interface IKeysProvider
    {
        /// <summary>
        /// Создать пару ключей
        /// </summary>
        /// <param name="curve">Эллиптическая кривая</param>
        /// <returns></returns>
        KeyPair CreateKeyPair(Curve curve);
    }
}