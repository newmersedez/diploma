using System.Collections.Generic;
using System.Linq;

namespace Diploma.Storage.Common.Verifier
{
    /// <summary>
    /// Строитель ошибок запроса
    /// </summary>
    public class ErrorBuilder
    {
        private Dictionary<string, IEnumerable<string>> _errors;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ErrorBuilder()
        {
            _errors = new Dictionary<string, IEnumerable<string>>();
        }

        /// <summary>
        /// Проверить наличие ошибок
        /// </summary>
        /// <returns></returns>
        public bool HasErrors() => _errors.Count > 0;
        
        /// <summary>
        /// Добавить ошибку
        /// </summary>
        /// <param name="key">Ключ ошибки</param>
        /// <param name="message">Сообщение</param>
        public void Add(string key, string message)
        {
            if (_errors.ContainsKey(key))
            {
                _errors[key] = _errors[key].Append(message);
            }
            else
            {
                _errors.Add(key, new[] { message });
            }
        }
    }
}