using System.Numerics;
using Diploma.ECC.Math.Entities;

namespace Diploma.Storage.Services.Signature.Request
{
    /// <summary>
    /// Запрос на проверку электронной подписи
    /// </summary>
    public sealed class VerifySignatureRequest
    {
        /// <summary>
        /// Публичный ключ
        /// </summary>
        public string X { get; set; }

        public string Y { get; set; }

        public string R { get; set; }

        public string S { get; set; }

        public string Folder { get; set; }

        public string Name { get; set; }
}
}