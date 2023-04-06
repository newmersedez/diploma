using System;
using System.Threading.Tasks;
using Diploma.Storage.Services.Signature;
using Diploma.Storage.Services.Signature.Request;
using Diploma.Storage.Services.Signature.Response;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Storage.Controllers
{
    /// <summary>
    /// Контроллер электронной подписи
    /// </summary>
    [ApiController]
    [Route("signature")]
    public sealed class SignatureController : ControllerBase
    {
        /// <summary>
        /// Подписать файл
        /// </summary>
        /// <param name="signatureService">Сервис электронной подписи</param>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        [HttpPost]
        [Route("sign")]
        public async Task<SignFileResponse> SignFileAsync(
            [FromServices] ISignatureService signatureService,
            SignFileRequest request)
        {
            if (signatureService == null) throw new ArgumentNullException(nameof(signatureService));

            var response = await signatureService.SignFileAsync(request);

            return response;
        }
        
        /// <summary>
        /// Проверить электронную подпись
        /// </summary>
        /// <param name="signatureService">Сервис электронной подписи</param>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        [HttpPost]
        [Route("verify")]
        public async Task<bool> VerifySignatureAsync(
            [FromServices] ISignatureService signatureService,
            VerifySignatureRequest request)
        {
            if (signatureService == null) throw new ArgumentNullException(nameof(signatureService));

            var response = await signatureService.VerifySignatureAsync(request);

            return response;
        }
    }
}