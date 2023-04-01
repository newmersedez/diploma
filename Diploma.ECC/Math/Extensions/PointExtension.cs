using System;
using System.Numerics;
using Diploma.Ecc.Math.Entities;

namespace Diploma.Ecc.Math.Extensions
{
    /// <summary>
    /// Операции с точкой эллиптической кривой
    /// </summary>
    public static class PointExtension
    {
        /// <summary>
        /// Проверить, является ли точка бесконечной
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns></returns>
        public static bool IsInfinityPoint(this Point point)
        {
            return point == Point.InfinityPoint;
        }

        /// <summary>
        /// Скалярное умножение точки
        /// </summary>
        /// <param name="times">Коэффициент</param>
        /// <param name="point">Точка</param>
        /// <returns></returns>
        public static Point Multiply(this Point point, BigInteger times)
        {
            if (point.IsInfinityPoint() || times % point.Curve.Parameters.N == 0)
            {
                return Point.InfinityPoint;
            }

            if (!point.IsOnCurve())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Точка не принадлежит эллиптической кривой");
            }

            if (times < 0)
            {
                return Multiply(point.Negate(), -times);
            }
            
            var resultPoint = Point.InfinityPoint;
            var addend = point;

            while (times != 0)
            {
                if ((times & 1) == 1)
                {
                    resultPoint = resultPoint.Add(addend);
                }

                addend = addend.Add(addend);

                times >>= 1;
            }

            if (!point.Curve.CheckPointIsOnCurve(resultPoint))
            {
                throw new ArgumentOutOfRangeException(nameof(resultPoint), "Точка не принадлежит эллиптической кривой");
            }

            return resultPoint;
        }

        /// <summary>
        /// Сложение точек
        /// </summary>
        /// <param name="first">Первая точка</param>
        /// <param name="second">Вторая точка</param>
        /// <returns></returns>
        public static Point Add(this Point first, Point second)
        {
            if (first.IsInfinityPoint())
            {
                return second;
            }

            if (second.IsInfinityPoint())
            {
                return first;
            }

            if (first.Curve != second.Curve)
            {
                throw new ArgumentException("Эллиптические кривые различаются");
            }
            
            var curve = first.Curve;
            if (!curve.CheckPointIsOnCurve(first))
            {
                throw new ArgumentOutOfRangeException(nameof(first), "Точка 1 не принадлежит эллиптической кривой");
            }
            
            if (!curve.CheckPointIsOnCurve(second))
            {
                throw new ArgumentOutOfRangeException(nameof(first), "Точка 2 не принадлежит эллиптической кривой");
            }

            BigInteger temporary;

            if (first.X == second.X)
            {
                if (first.Y != second.Y)
                {
                    return Point.InfinityPoint;
                }

                temporary = (3 * BigInteger.Pow(first.X, 2) + curve.Parameters.A) * (2 * first.Y).ModuleInverse(curve.Parameters.P);
            }
            else
                temporary = (first.Y - second.Y) * (first.X - second.X).ModuleInverse(curve.Parameters.P);

            var newPointX = BigInteger.Pow(temporary, 2) - first.X - second.X;
            var newPointY = first.Y + temporary * (newPointX - first.X);
            
            return new Point(newPointX.Module(curve.Parameters.P), (-newPointY).Module(curve.Parameters.P), curve);
        }

        /// <summary>
        /// Найти обратную величину
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns></returns>
        public static Point Negate(this Point point)
        {
            if (!point.IsOnCurve())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Точка не принадлежит эллиптической кривой");
            }

            if (point.IsInfinityPoint())
            {
                return point;
            }

            var result = new Point(point.X, (-point.Y).Module(point.Curve.Parameters.P), point.Curve);

            if (!result.IsOnCurve())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Точка не принадлежит эллиптической кривой");
            }

            return result;
        }
        
        /// <summary>
        /// Проверить принадлежность точки к кривой
        /// </summary>
        private static bool IsOnCurve(this Point point) => point.Curve.CheckPointIsOnCurve(point);
    }
}