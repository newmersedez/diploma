using Diploma.ECC.Enums;
using Diploma.ECC.Extensions;

namespace Diploma.ECC.Math
{
    /// <summary>
    /// Эллиптическая кривая
    /// </summary>
    public class Curve
    {
        # region Properties

        /// <summary>
        /// Параметры эллиптической кривой
        /// </summary>
        public CurveParameters Parameters { get; set; }

        # endregion

        # region Constructors

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Вид эллиптической кривой</param>
        public Curve(CurveName name)
        {
            Parameters = name.GetParameters(this);
        }

        # endregion
        
    }
}