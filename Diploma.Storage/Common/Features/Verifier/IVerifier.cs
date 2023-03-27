using System.Threading.Tasks;

namespace Diploma.Storage.Common.Features.Verifier
{
    /// <summary>
    /// Интерфейс валидатора
    /// </summary>
    public interface IVerifier<TContext>
        where TContext: class
    {
        /// <summary>
        /// Верификация
        /// </summary>
        /// <param name="context">Контекст проверки</param>
        /// <returns></returns>
        Task VerifyAsync(TContext context);
    }
}