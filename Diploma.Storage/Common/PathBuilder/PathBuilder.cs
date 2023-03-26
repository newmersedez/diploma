using System.Collections.Generic;

namespace Diploma.Storage.Common.PathBuilder
{
    /// <summary>
    /// Сервис построения системного пути
    /// </summary>
    public sealed class PathBuilder : IPathBuilder
    {
        /// <summary>
        /// Текущий путь
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Добавить значение в путь
        /// </summary>
        /// <param name="path">Путь</param>
        public string Append(string path)
        {
            Path = System.IO.Path.Join(Path, path);
            
            return Path;
        }

        /// <summary>
        /// Добавить несколько значений в путь
        /// </summary>
        /// <param name="paths">Пути</param>
        public string Append(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                Path = System.IO.Path.Join(Path, path);
            }

            return Path;
        }

        /// <summary>
        /// Очистить путь
        /// </summary>
        public void Clear()
        {
            Path = string.Empty;
        }
    }
}