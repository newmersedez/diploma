using System.Collections.Generic;
using System.Linq;

namespace Diploma.Bll.Common.Verify
{
    /// <summary>
    /// Строитель ошибок запроса
    /// </summary>
    public sealed class ErrorBuilder
    {
        private readonly Dictionary<string, List<string>> _errors;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ErrorBuilder()
        {
            _errors = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Наличие ошибок
        /// </summary>
        /// <returns></returns>
        public bool HasErrors() => _errors is { Count: > 0 };

        /// <summary>
        /// Добавить ошибку
        /// </summary>
        /// <param name="key">Ключ ошибки</param>
        /// <param name="error">Текст ошибки</param>
        /// <returns></returns>
        public ErrorBuilder Add(string key, string error)
        {
            if (!_errors.ContainsKey(key))
            {
                _errors[key] = new List<string>();
            }
            _errors[key].Add(error);

            return this;
        }

        /// <summary>
        /// Получить ошибки
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> Build()
        {
            return _errors.ToDictionary(x => x.Key, x => x.Value);
        }

    }
}