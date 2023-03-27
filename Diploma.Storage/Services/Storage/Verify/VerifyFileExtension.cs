using System.Threading.Tasks;
using Diploma.Storage.Common.Features.Verifier;
using Diploma.Storage.Services.Storage.Verify.Context;

namespace Diploma.Storage.Services.Storage.Verify
{
    /// <summary>
    /// Верификация расширения файла
    /// </summary>
    public sealed class VerifyFileExtension : IVerify<UploadFileContext>
    {
        /// <summary>
        /// Верификация при загрузке файла
        /// </summary>
        /// <param name="context">Контекст</param>
        /// <param name="errorBuilder">Строитель ошибок запроса</param>
        /// <returns></returns>
        public Task VerifyAsync(UploadFileContext context, ErrorBuilder errorBuilder)
        {
            // TODO: расширения файлов в appsettings.json
            return Task.CompletedTask;
        }
    }
}