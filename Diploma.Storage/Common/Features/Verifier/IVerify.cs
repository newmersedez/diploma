using System.Threading.Tasks;

namespace Diploma.Storage.Common.Features.Verifier
{
    /// <summary>
    /// Интерфейс верификации
    /// </summary>
    public interface IVerify<in TContext>
        where TContext : class
    {
        /// <summary>
        /// Верификация
        /// </summary>
        /// <param name="context">Контекст</param>
        /// <param name="errorBuilder">Строитель ошибок запроса</param>
        /// <returns></returns>
        Task VerifyAsync(TContext context, ErrorBuilder errorBuilder);
    }
}