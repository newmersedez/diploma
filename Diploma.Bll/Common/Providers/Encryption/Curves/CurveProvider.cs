using System;
using Diploma.Ecc.Math.Entities;
using Diploma.Ecc.Math.Enums;
using Microsoft.Extensions.Configuration;

namespace Diploma.Bll.Common.Providers.Encryption.Curves
{
    /// <summary>
    /// Провайдер эллиптической кривой
    /// </summary>
    public sealed class CurveProvider : ICurveProvider
    {
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CurveProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Получить кривую
        /// </summary>
        /// <returns></returns>
        public Curve GetCurve()
        {
            var curveName = _configuration.GetValue<string>("Encryption:Curve");
            var curveEnum = (CurveName)Enum.Parse(typeof(CurveName), curveName);
            
            return new Curve(curveEnum);
        }
    }
}