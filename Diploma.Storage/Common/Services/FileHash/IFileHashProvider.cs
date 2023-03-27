namespace Diploma.Storage.Common.Services.FileHash
{
    /// <summary>
    /// Провайдер хэш-суммы файла
    /// </summary>
    public interface IFileHashProvider
    {
        /// <summary>
        /// Посчитать хэш сумму файла
        /// </summary>
        /// <param name="content">Содержимое файла</param>
        /// <returns></returns>
        string CalculateHashSum(byte[] content);
    }
}