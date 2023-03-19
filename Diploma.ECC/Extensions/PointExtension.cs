using System;
using System.Numerics;
using Diploma.ECC.Math;

namespace Diploma.ECC.Extensions
{
    public static class PointExtension
    {
        /// <summary>
        /// Скалярное умножение точки
        /// </summary>
        /// <param name="scalar">Коэффициент</param>
        /// <param name="point">Точка для умножения</param>
        /// <returns></returns>
        public static Point Multiply(this Point point, BigInteger scalar)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сложение точек
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Point Add(this Point point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Найти обратную величину
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns></returns>
        public static Point Negate(this Point point)
        {
            throw new NotImplementedException();
        }
    }
}