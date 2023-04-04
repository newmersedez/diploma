using Diploma.Ecc.Encryption.Key;
using Diploma.Ecc.Math.Entities;

namespace Diploma.Bll.Common.Providers.Encryption.Keys
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