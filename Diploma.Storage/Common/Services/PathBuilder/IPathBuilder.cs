using System.Collections.Generic;

namespace Diploma.Storage.Common.Services.PathBuilder
{
    /// <summary>
    /// Интерфейс сервиса построения пути
    /// </summary>
    public interface IPathBuilder
    {
        public string Path { get; }

        /// <summary>
        /// Добавить значение в путь
        /// </summary>
        /// <param name="path">Путь</param>
        public string Append(string path);

        /// <summary>
        /// Добавить несколько значений в путь
        /// </summary>
        /// <param name="paths">Пути</param>
        public string Append(IEnumerable<string> paths);

        /// <summary>
        /// Очистить путь
        /// </summary>
        public void Clear();
    }
}