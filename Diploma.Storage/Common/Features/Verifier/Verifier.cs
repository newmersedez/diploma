using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Diploma.Storage.Common.Exceptions;

namespace Diploma.Storage.Common.Features.Verifier
{
    /// <summary>
    /// Валидатор запроса
    /// </summary>
    /// <typeparam name="TContext">Контекст проверки</typeparam>
    public sealed class Verifier<TContext> : IVerifier<TContext> 
        where TContext : class
    {
        private readonly List<IVerify<TContext>> _verifies;
        private const string ERROR_MESSAGE = "Ошибка валидации запроса";

        /// <summary>
        /// Конструктор
        /// </summary>
        public Verifier(List<IVerify<TContext>> verifies)
        {
            _verifies = verifies;
        }

        /// <summary>
        /// Верификация
        /// </summary>
        /// <param name="context">Контекст проверки</param>
        /// <returns></returns>
        public async Task VerifyAsync(TContext context)
        {
            var errorBuilder = new ErrorBuilder();

            foreach (var verify in _verifies)
            {
                await verify.VerifyAsync(context, errorBuilder);
            }

            if (errorBuilder.HasErrors())
            {
                throw new RequestException(HttpStatusCode.BadRequest, ERROR_MESSAGE, errorBuilder.Build());
            }
        }
    }
}