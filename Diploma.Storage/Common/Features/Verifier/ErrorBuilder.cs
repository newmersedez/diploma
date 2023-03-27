using System.Collections.Generic;

namespace Diploma.Storage.Common.Features.Verifier
{
    /// <summary>
    /// Строитель ошибок запроса
    /// </summary>
    public class ErrorBuilder
    {
        private Dictionary<string, List<string>> _errors;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ErrorBuilder()
        {
            _errors = new Dictionary<string, List<string>>();
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
            if (!_errors.ContainsKey(key))
            {
                _errors.Add(key, new List<string> { message });
                return;
            }
            
            _errors[key].Add(message);
        }

        /// <summary>
        /// Получить ошибки верификации
        /// </summary>
        public Dictionary<string, List<string>> Build()
        {
            return _errors;
        }
    }
}