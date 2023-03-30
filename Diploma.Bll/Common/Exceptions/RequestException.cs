using System;
using System.Collections.Generic;
using System.Net;

namespace Diploma.Bll.Common.Exceptions
{
    /// <summary>
    /// Ошибка выполнения запроса
    /// </summary>
    public class RequestException : Exception
    {
        /// <summary>
        /// Статус ошибки
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Подробное описание ошибки
        /// </summary>
        public Dictionary<string, List<string>> Details { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RequestException(HttpStatusCode statusCode, string message, Dictionary<string, List<string>> details = null)
            : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}