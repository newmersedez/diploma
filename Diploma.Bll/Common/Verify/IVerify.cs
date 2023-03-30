using System.Threading.Tasks;

namespace Diploma.Bll.Common.Verify
{
    /// <summary>
    /// Интерфейс верификации контекста
    /// </summary>
    public interface IVerify<in TValidateContext>
    {
        /// <summary>
        /// Верификация
        /// </summary>
        /// <param name="context">Контекст</param>
        /// <param name="errorBuilder">Строитель ошибок запроса</param>
        /// <returns></returns>
        Task VerifyAsync(TValidateContext context, ErrorBuilder errorBuilder);
    }
}