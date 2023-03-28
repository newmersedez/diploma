namespace Diploma.Storage.Services.Signature.Response
{
    /// <summary>
    /// Ответ при получении электронной подписи
    /// </summary>
    public sealed class SignFileResponse
    {
        /// <summary>
        /// Электронная подпись
        /// </summary>
        public Common.Responses.Signature Signature { get; set; }
    }
}