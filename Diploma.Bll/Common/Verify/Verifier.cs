using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Diploma.Bll.Common.Exceptions;

namespace Diploma.Bll.Common.Verify
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public sealed class Verifier<TValidateContext> : IVerifier<TValidateContext>
    {
        private const string REQUEST_VALIDATION_ERROR = "Ошибка валидации запроса";
        private readonly IEnumerable<IVerify<TValidateContext>> _verifies;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="verifies">Верификации</param>
        public Verifier(IEnumerable<IVerify<TValidateContext>> verifies)
        {
            _verifies = verifies ?? throw new ArgumentNullException(nameof(verifies));
        }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="context">Контекст валидации</param>
        /// <returns></returns>
        public async Task VerifyAsync(TValidateContext context)
        {
            var errorBuilder = new ErrorBuilder();

            foreach (var verify in _verifies)
            {
                await verify.VerifyAsync(context, errorBuilder);
            }

            if (errorBuilder.HasErrors())
            {
                throw new RequestException(HttpStatusCode.BadRequest, REQUEST_VALIDATION_ERROR, errorBuilder.Build());
            }
        }
    }
}