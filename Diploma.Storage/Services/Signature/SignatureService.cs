using System;
using System.IO;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Diploma.ECC.Math.Extensions;
using Diploma.ECC.Math.Utils;
using Diploma.Storage.Common.Exceptions;
using Diploma.Storage.Services.Signature.Request;
using Diploma.Storage.Services.Signature.Response;
using Microsoft.Extensions.Configuration;

namespace Diploma.Storage.Services.Signature
{
    /// <summary>
    /// Сервис электронной подписи
    /// </summary>
    public sealed class SignatureService : ISignatureService
    {
        private readonly string STORAGE_ROOT;
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        public SignatureService(IConfiguration configuration)
        {
            STORAGE_ROOT = configuration.GetValue<string>("Storage:Root");
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Подписать файл
        /// </summary>
        /// <param name="request">Запрос на подписание</param>
        /// <returns></returns>
        public async Task<SignFileResponse> SignFileAsync(SignFileRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: Верификация параметров запроса
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), STORAGE_ROOT, request.Folder, request.Name);
            if (!File.Exists(filePath))
            {
                throw new RequestException(HttpStatusCode.NotFound, "Файл по указанному пути не существует");
            }
            var content = await File.ReadAllBytesAsync(filePath);
            
            var curveName = _configuration.GetValue<string>("Signature:Curve");
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
            var s = kInverse * (new BigInteger(content) + r * BigInteger.Parse(request.PrivateKey)) % curve.Parameters.N;

            return new SignFileResponse
            {
                S = s.ToString(),
                R = r.ToString()
            };
        }

        /// <summary>
        /// Проверить электронную подпись
        /// </summary>
        /// <param name="request">Запрос на проверку электронной подписи</param>
        /// <returns></returns>
        public async Task<bool> VerifySignatureAsync(VerifySignatureRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            // TODO: Верификация параметров запроса
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), STORAGE_ROOT, request.Folder, request.Name);
            if (!File.Exists(filePath))
            {
                throw new RequestException(HttpStatusCode.NotFound, "Файл по указанному пути не существует");
            }
            var content = await File.ReadAllBytesAsync(filePath);
            
            var curveName = _configuration.GetValue<string>("Signature:Curve");
            var curveEnum = (CurveName)Enum.Parse(typeof(CurveName), curveName);
            var curve = new Curve(curveEnum);

            var sInverse = BigInteger.Parse(request.S);
            sInverse = sInverse.ModuleInverse(curve.Parameters.N);
            var u = new BigInteger(content) * sInverse % curve.Parameters.N;
            var v = BigInteger.Parse(request.R) * sInverse % curve.Parameters.N;
            var cPoint = curve.Parameters.G.Multiply(u).Add(
                new Point(BigInteger.Parse(request.X), BigInteger.Parse(request.Y), curve).Multiply(v));
            return cPoint.X == BigInteger.Parse(request.R);
        }
    }
}