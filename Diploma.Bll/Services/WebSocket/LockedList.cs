using System.Collections;
using System.Collections.Generic;

namespace Diploma.Bll.Services.WebSocket
{
    /// <summary>
    /// Concurrency список
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    public class LockedList<T> : ICollection<T>
    {
        private readonly List<T> _list = new();
        private readonly object _lock = new();

        /// <summary>
        /// Добавить объект
        /// </summary>
        /// <param name="obj">Объект</param>
        public void Add(T obj)
        {
            lock (_lock)
            {
                _list.Add(obj);
            }
        }

        /// <summary>
        /// Очистить список
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        /// <summary>
        /// Проверить существование объекта в списке
        /// </summary>
        /// <param name="item">Объект</param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _list.Contains(item);
            }
        }

        /// <summary>
        /// Скопировать список
        /// </summary>
        /// <param name="array">Массив</param>
        /// <param name="arrayIndex">Индекс начала</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Удалить объект
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        public bool Remove(T obj)
        {
            lock (_lock)
            {
                return _list.Remove(obj);
            }
        }

        /// <summary>
        /// Количество записей
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }

        /// <summary>
        /// Только для чтения
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Получить Enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                var copy = new List<T>(_list);
                return copy.GetEnumerator();
            }
        }

        /// <summary>
        /// Получить Enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}