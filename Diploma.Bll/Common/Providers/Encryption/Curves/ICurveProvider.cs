
using Diploma.Ecc.Math.Entities;

namespace Diploma.Bll.Common.Providers.Encryption.Curves
{
    /// <summary>
    /// Провайдер эллиптической кривой
    /// </summary>
    public interface ICurveProvider
    {
        /// <summary>
        /// Получить кривую
        /// </summary>
        /// <returns></returns>
        Curve GetCurve();
    }
}