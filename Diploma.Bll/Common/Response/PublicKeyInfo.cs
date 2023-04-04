namespace Diploma.Bll.Common.Response
{
    /// <summary>
    /// Информация о публичном ключе эллиптической криптографии
    /// </summary>
    public sealed class PublicKeyInfo
    {
        /// <summary>
        /// Параметр X
        /// </summary>
        public string X { get; set; }

        /// <summary>
        /// Параметр Y
        /// </summary>
        public string Y { get; set; }
    }
}