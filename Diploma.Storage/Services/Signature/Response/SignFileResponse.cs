using System.Numerics;

namespace Diploma.Storage.Services.Signature.Response
{
    /// <summary>
    /// Ответ при получении электронной подписи
    /// </summary>
    public sealed class SignFileResponse
    {
        /// <summary>
        /// R
        /// </summary>
        public string R { get; set; }

        /// <summary>
        /// S
        /// </summary>
        public string S { get; set; }
    }
}