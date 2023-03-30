using System.Threading.Tasks;

namespace Diploma.Bll.Common.Verify
{
    /// <summary>
    /// Интерфейс валидатора
    /// </summary>
    public interface IVerifier<in TValidateContext>
    {
        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="context">Контекст</param>
        /// <returns></returns>
        Task VerifyAsync(TValidateContext context);
    }
}