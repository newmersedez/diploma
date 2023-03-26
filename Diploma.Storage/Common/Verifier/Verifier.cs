using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Diploma.Storage.Common.Verifier
{
    /// <summary>
    /// Cервиса проверки
    /// </summary>
    public class Verifier<TContext>
        where TContext: class, new()
    {
        private IEnumerable<IVerify<TContext>> _verifies;
        private ErrorBuilder _errorBuilder;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Verifier()
        {
            _errorBuilder = new ErrorBuilder();
        }

        /// <summary>
        /// Добавить верификацию
        /// </summary>
        public void AddVerification(IVerify<TContext> verify)
        {
            _verifies = _verifies.Append(verify);
        }

        /// <summary>
        /// Верификация 
        /// </summary>
        /// <returns></returns>
        public Task VerifyAsync()
        {
            foreach (var verify in _verifies)
            {
                verify.VerifyAsync(new TContext(), _errorBuilder);
            }

            if (_errorBuilder.HasErrors())
            {
                throw new BadHttpRequestException("Validation failed");
            }

            return Task.CompletedTask;
        }
    }
}