using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Diploma.Gateway.Common
{
    /// <summary>
    /// Точка назначения
    /// </summary>
    public sealed class Destination
    {
        public string Uri { get; set; }
        public bool RequiresAuthentication { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uri">Путь</param>
        /// <param name="requiresAuthentication">Требует аутентификации</param>
        public Destination(string uri, bool requiresAuthentication = false)
        {
            Uri = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        /// <summary>
        /// Создать точку назначения
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        public string CreateDestinationUri(HttpRequest request)
        {
            var requestPath = request.Path.ToString();
            var queryString = request.QueryString.ToString();

            var endpoint = "";
            var endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1)
            {
                endpoint = endpointSplit[1];
            }

            return Uri + endpoint + queryString;
        }

        /// <summary>
        /// Отправить запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequest request)
        {
            {
                string requestContent;
                using (var receiveStream = request.Body)
                {
                    using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        requestContent = readStream.ReadToEnd();
                    }
                }

                var client = new HttpClient();
                var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request));
                var response = await client.SendAsync(newRequest);

                return response;
            }
        }
    }
}