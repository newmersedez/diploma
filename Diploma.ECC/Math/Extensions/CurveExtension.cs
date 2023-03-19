using System.Numerics;
using Diploma.ECC.Math.Entities;

namespace Diploma.ECC.Math.Extensions
{
    /// <summary>
    /// Операции с эллиптической кривой
    /// </summary>
    public static class CurveExtension
    {
        /// <summary>
        /// Проверить принадлежность точки к кривой
        /// </summary>
        /// <param name="curve">Эллиптическая кривая</param>
        /// <param name="point">Точка</param>
        /// <returns></returns>
        public static bool CheckPointIsOnCurve(this Curve curve, Point point)
        {
            return point.IsInfinityPoint() 
                   || (BigInteger.Pow(point.Y, 2) 
                       - BigInteger.Pow(point.X, 3) 
                       - curve.Parameters.A * point.X 
                       - curve.Parameters.B) % curve.Parameters.P == 0;
        }
    }
}