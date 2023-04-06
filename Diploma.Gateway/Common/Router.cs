using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Diploma.Gateway.Common
{
    /// <summary>
    /// Класс роутера
    /// </summary>
    public class Router {

        /// <summary>
        /// Пути
        /// </summary>
        public List<Route> Routes { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="routeConfigFilePath">Путь к файлу конфигурации</param>
        public Router(string routeConfigFilePath)
        {
            var router = JsonLoader.LoadFromFile<dynamic>(routeConfigFilePath);

            Routes = JsonLoader.Deserialize<List<Route>>(Convert.ToString(router.routes)
            );
        }
        
        /// <summary>
        /// Перенаправить запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> RouteRequestAsync(HttpRequest request)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[1];

            Destination destination;
            try
            {
                destination = Routes.First(r => r.Endpoint.Equals(basePath)).Destination;
            }
            catch
            {
                return ConstructErrorMessage("The path could not be found.");
            }

            if (destination.RequiresAuthentication)
            {
                string token = request.Headers["token"];
                var _ = request.Query.Append(new KeyValuePair<string, StringValues>("token", new StringValues(token)));
            }

            return await destination.SendRequestAsync(request);
        }

        /// <summary>
        /// Получить ошибку
        /// </summary>
        /// <param name="error">Ошибка</param>
        /// <returns></returns>
        private HttpResponseMessage ConstructErrorMessage(string error)
        {
            var errorMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(error)
            };
            return errorMessage;
        }
    }
}