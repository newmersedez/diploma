using System.Threading.Tasks;
using Diploma.Storage.Services.Signature.Request;
using Diploma.Storage.Services.Signature.Response;

namespace Diploma.Storage.Services.Signature
{
    /// <summary>
    /// Интерфейс сервиса электронной подписи
    /// </summary>
    public interface ISignatureService
    {
        /// <summary>
        /// Подписать файл
        /// </summary>
        /// <param name="request">Запрос на подписание</param>
        /// <returns></returns>
        Task<SignFileResponse> SignFileAsync(SignFileRequest request);

        /// <summary>
        /// Проверить электронную подпись
        /// </summary>
        /// <param name="request">Запрос на проверку электронной подписи</param>
        /// <returns></returns>
        Task<bool> VerifySignatureAsync(VerifySignatureRequest request);
    }
}