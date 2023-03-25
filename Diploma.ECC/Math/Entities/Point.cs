using System.Numerics;

namespace Diploma.ECC.Math.Entities
{
    /// <summary>
    /// Точка на эллиптической кривой
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Координата по X
        /// </summary>
        public BigInteger X { get; set; }

        /// <summary>
        /// Координата по Y
        /// </summary>
        public BigInteger Y { get; set; }

        /// <summary>
        /// Эллиптическая кривая
        /// </summary>
        public Curve Curve { get; set; }

        /// <summary>
        /// Бесконечная точка
        /// </summary>
        public static Point InfinityPoint => null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x">Координата по X</param>
        /// <param name="y">Координата по Y</param>
        /// <param name="curve">Эллиптическая кривая</param>
        public Point(BigInteger x, BigInteger y, Curve curve)
        {
            X = x;
            Y = y;
            Curve = curve;
        }
    }
}