using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Diploma.ECC.Math.Extensions;
using Diploma.ECC.Math.Utils;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Diploma.Signature.Services
{
    /// <summary>
    /// Сервис электронной подписи
    /// </summary>
    public class SignatureService : Signature.SignatureBase
    {
        private readonly ILogger<SignatureService> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger">Логгер</param>
        /// <param name="configuration">Конфигурация</param>
        public SignatureService(ILogger<SignatureService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Подписать документ электронной подписью
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="context">Контекст</param>
        /// <returns></returns>
        public override Task<SignFileResponse> SignFileAsync(SignFileRequest request, ServerCallContext context)
        {
            var privateKey = BigInteger.Parse(request.PrivateKey);
            
            var curveName = _configuration.GetValue<string>("Cryptography:Curve");
            var curveEnum = (CurveName)Enum.Parse(typeof(CurveName), curveName);
            var curve = new Curve(curveEnum);

            BigInteger r, k;
            do
            {
                k = BigIntGenerator.GenerateRandom(curve.Parameters.N);
                var rPoint = curve.Parameters.G.Multiply(k);
                r = rPoint.X % curve.Parameters.N;
            } while (r == 0);

            var kInverse = k.ModuleInverse(curve.Parameters.N);
            var s = kInverse * (new BigInteger(request.Content.ToByteArray()) + r * privateKey) % curve.Parameters.N;

            return Task.FromResult(new SignFileResponse()
            {
                S = ByteString.CopyFrom(s.ToByteArray()),
                R =  ByteString.CopyFrom(r.ToByteArray()),
            });
        }

        /// <summary>
        /// Проверить электронную подпись
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="context">Контекст</param>
        /// <returns></returns>
        public override Task<VerifySignatureResponse> VerifySignatureAsync(VerifySignatureRequest request, ServerCallContext context)
        {
            var curveName = _configuration.GetValue<string>("Cryptography:Curve");
            var curveEnum = (CurveName)Enum.Parse(typeof(CurveName), curveName);
            var curve = new Curve(curveEnum);

            var sInverse = new BigInteger(request.S.ToByteArray());
            sInverse = sInverse.ModuleInverse(curve.Parameters.N);
            var u = new BigInteger(request.Content.ToByteArray()) * sInverse % curve.Parameters.N;
            var v = new BigInteger(request.R.ToByteArray()) * sInverse % curve.Parameters.N;
            var cPoint = curve.Parameters.G.Multiply(u).Add(
                new Point(new BigInteger(request.X.ToByteArray()), new BigInteger(request.Y.ToByteArray()), curve).Multiply(v));
            return Task.FromResult(new VerifySignatureResponse
            {
                IsVerified = cPoint.X == new BigInteger(request.R.ToByteArray())
            });
        }
    }
}