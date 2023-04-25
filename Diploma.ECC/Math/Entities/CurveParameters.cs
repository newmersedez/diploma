using System.Numerics;
using Diploma.Ecc.Math.Enums;

namespace Diploma.Ecc.Math.Entities
{
    /// <summary>
    /// Параметры эллиптической кривой
    /// </summary>
    public sealed class CurveParameters
    {
        /// <summary>
        /// Вид эллиптической кривой
        /// </summary>
        public CurveName Name { get; set; }

        /// <summary>
        /// Размер конечного поля
        /// </summary>
        public BigInteger P { get; set; }

        /// <summary>
        /// Коэффициент А уравнения эллиптический кривой
        /// </summary>
        public BigInteger A { get; set; }

        /// <summary>
        /// Коэффициент B уравнения эллиптической кривой
        /// </summary>
        public BigInteger B { get; set; }

        /// <summary>
        /// Базовая точка, генерирующая подгруппу
        /// </summary>
        public Point G { get; set; }

        /// <summary>
        /// Порядок подгруппы
        /// </summary>
        public BigInteger N { get; set; }

        /// <summary>
        /// Кофактор подгруппы
        /// </summary>
        public short H { get; set; }
        
        /// <summary>
        /// Длина
        /// </summary>
        public uint Length { get; set; }
    }
}