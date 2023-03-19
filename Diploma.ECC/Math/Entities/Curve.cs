using Diploma.ECC.Math.Enums;
using Diploma.ECC.Math.Extensions;

namespace Diploma.ECC.Math.Entities
{
    /// <summary>
    /// Эллиптическая кривая
    /// </summary>
    public class Curve
    {
        /// <summary>
        /// Параметры эллиптической кривой
        /// </summary>
        public CurveParameters Parameters { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Вид эллиптической кривой</param>
        public Curve(CurveName name)
        {
            Parameters = name.GetParameters(this);
        }
    }
}