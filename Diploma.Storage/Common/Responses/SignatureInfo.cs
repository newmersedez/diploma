namespace Diploma.Storage.Common.Responses
{
    /// <summary>
    /// Сущность электронной подписи
    /// </summary>
    public sealed class SignatureInfo
    {
        /// <summary>
        /// Параметр R
        /// </summary>
        public string R { get; set; }

        /// <summary>
        /// Параметр S
        /// </summary>
        public string S { get; set; }
    }
}