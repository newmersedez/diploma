using Diploma.ECC.Enums;
using Diploma.ECC.Extensions;

namespace Diploma.ECC.Math
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