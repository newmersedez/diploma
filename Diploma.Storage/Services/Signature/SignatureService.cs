using System;
using System.IO;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Diploma.Ecc.Math.Entities;
using Diploma.Ecc.Math.Enums;
using Diploma.Ecc.Math.Extensions;
using Diploma.Ecc.Math.Utils;
using Diploma.Storage.Common.Exceptions;
using Diploma.Storage.Common.Responses;
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
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), STORAGE_ROOT, request.FileHash);
            if (!Directory.Exists(folderPath))
            {
                throw new RequestException(HttpStatusCode.BadRequest, "Файл не существует");
            }

            var signatureFilePath = Path.Combine(folderPath, _configuration.GetValue<string>("Signature:File"));
            var fileHash = new BigInteger(Encoding.UTF8.GetBytes(request.FileHash));
            var privateKey = BigInteger.Parse(request.PrivateKey);

            return new SignFileResponse
            {
                Signature = await SignFileAsync(signatureFilePath, fileHash, privateKey)
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
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), STORAGE_ROOT, request.FileHash);
            if (!Directory.Exists(folderPath))
            {
                throw new RequestException(HttpStatusCode.BadRequest, "Файл не существует");
            }
            
            var signatureFilePath = Path.Combine(folderPath, _configuration.GetValue<string>("Signature:File"));
            if (!File.Exists(signatureFilePath))
            {
                throw new RequestException(HttpStatusCode.BadRequest, "Электронной подписи не существует");
            }

            var fileHash = new BigInteger(Encoding.UTF8.GetBytes(request.FileHash));
            var publicKey = new Point(BigInteger.Parse(request.PublicKey.X), BigInteger.Parse(request.PublicKey.Y), null);

            return await VerifySignatureAsync(request.Signature, fileHash, publicKey);
        }

        /// <summary>
        /// Подписать файл
        /// </summary>
        /// <param name="path">Путь к файлу сохранения</param>
        /// <param name="hash">Хэш-сумма файла</param>
        /// <param name="privateKey">Приватный ключ</param>
        private async Task<SignatureInfo> SignFileAsync(string path, BigInteger hash, BigInteger privateKey)
        {
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
            var s = kInverse * (hash + r * privateKey) % curve.Parameters.N;

            await SaveSignatureAsync(path, r, s);

            return new SignatureInfo
            {
                R = r.ToString(), 
                S = s.ToString()
            };
        }
        
        /// <summary>
        /// Проверить электронную подпись
        /// </summary>
        /// <param name="signature">Электронная подпись</param>
        /// <param name="hash">Хэш файла</param>
        /// <param name="publicKey">Публичный ключ</param>
        /// <returns></returns>
        private Task<bool> VerifySignatureAsync(SignatureInfo signature, BigInteger hash, Point publicKey)
        {
            var curveName = _configuration.GetValue<string>("Signature:Curve");
            var curveEnum = (CurveName)Enum.Parse(typeof(CurveName), curveName);
            var curve = new Curve(curveEnum);
            publicKey.Curve = curve;
            
            var r = BigInteger.Parse(signature.R);
            var s = BigInteger.Parse(signature.S);

            var sInverse = s.ModuleInverse(curve.Parameters.N);
            var u = hash * sInverse % curve.Parameters.N;
            var v = r * sInverse % curve.Parameters.N;
            var cPoint = curve.Parameters.G.Multiply(u).Add(publicKey.Multiply(v));
			
            return Task.FromResult(cPoint.X == r);
        }
        
        /// <summary>
        /// Сохранить электронную подпись
        /// </summary>
        /// <param name="path">Путь к файлу сохранения</param>
        /// <param name="r">Параметр R</param>
        /// <param name="s">Параметр S</param>
        private async Task SaveSignatureAsync(string path, BigInteger r, BigInteger s)
        {
            using var writer = new StreamWriter(path);

            var rString = Convert.ToBase64String(r.ToByteArray());
            var sString = Convert.ToBase64String(s.ToByteArray());

            await writer.WriteLineAsync(rString);
            await writer.WriteLineAsync(sString);
        }
    }
}