using System;
using System.Numerics;
using Diploma.ECC.Math.Entities;

namespace Diploma.ECC.Math.Extensions
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
        /// Проверить принадлежность точки к кривой
        /// </summary>
        public static bool IsOnCurve(this Point point) => point.Curve.CheckPointIsOnCurve(point);

        /// <summary>
        /// Скалярное умножение точки
        /// </summary>
        /// <param name="scalar">Коэффициент</param>
        /// <param name="point">Точка для умножения</param>
        /// <returns></returns>
        public static Point Multiply(this Point point, BigInteger scalar)
        {
            if (point.IsInfinityPoint() || scalar % point.Curve.Parameters.N == 0)
            {
                return Point.InfinityPoint;
            }

            if (!point.IsOnCurve())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Точка не принадлежит эллиптической кривой");
            }

            if (scalar < 0)
            {
                point = point.Negate();
                scalar = -scalar;
            }

            var resultPoint = Point.InfinityPoint;
            var end = point;

            while (scalar != 0)
            {
                if ((scalar & 1) == 1)
                {
                    if (resultPoint == null)
                    {
                        resultPoint = end;
                    }
                    
                    resultPoint = resultPoint.Add(end);
                }

                end = end.Add(end);

                scalar >>= 1;
            }

            if (!point.IsOnCurve())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Точка не принадлежит эллиптической кривой");
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
            if (first.Curve != second.Curve)
            {
                throw new ArgumentException("Эллиптические кривые различаются");
            }

            if (first.IsInfinityPoint())
            {
                return second;
            }

            if (second.IsInfinityPoint())
            {
                return first;
            }

            Curve commonCurve = first.Curve;
            if (!commonCurve.CheckPointIsOnCurve(first) || !commonCurve.CheckPointIsOnCurve(second))
            {
                throw new ArgumentOutOfRangeException(nameof(first), "Точка не принадлежит эллиптической кривой");
            }

            BigInteger temporary;

            if (first.X == second.X)
            {
                if (first.Y != second.Y)
                    return Point.InfinityPoint;

                var temp = 2 * first.Y;
                temporary = (3 * BigInteger.Pow(first.X, 2) + commonCurve.Parameters.A) * temp.ModuleInverse(commonCurve.Parameters.P);
            }
            else
                temporary = (first.Y - second.Y) * BigIntExtension.ModuleInverse(first.X - second.X, commonCurve.Parameters.P);

            BigInteger newX = BigInteger.Pow(temporary, 2) - first.X - second.X;
            BigInteger newY = first.Y + temporary * (newX - first.X);
            Point result = new Point(newX.Module(commonCurve.Parameters.P), -newY.Module(commonCurve.Parameters.P), commonCurve);

            return result;
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

            var result = new Point(point.X, -point.Y.Module(point.Curve.Parameters.P), point.Curve);

            if (!result.IsOnCurve())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Точка не принадлежит эллиптической кривой");
            }

            return result;
        }
    }
}