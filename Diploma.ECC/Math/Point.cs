using System;
using System.Numerics;

namespace Diploma.ECC.Math
{
    /// <summary>
    /// Точка на эллиптической кривой
    /// </summary>
    public class Point
    {
        # region Properties
        
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

        # endregion
        
        # region Constructors

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
        
        # endregion

        #region Methods

        /// <summary>
        /// Проверить принадлежность точки к кривой
        /// </summary>
        public void CheckPointIsOnCurve()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Получить строковое представление точки
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(X, Y);
        }

        #endregion
    }
}