using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Diploma.Client.Core.MVVM.ViewModel;
using Diploma.Client.MVVM.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Client.MVVM.ViewModel.Main
{
    /// <summary>
    /// Вьюмодель основного окна чата
    /// </summary>
    public sealed class ChatPageViewModel : ViewModelBase
    {
        private const string TOKEN = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImYyOWU0NTI0LTczYTctNDQ5MS05YmYxLTE5ZjcyNTM4ZTUyZCIsImlzcyI6ImNyeXB0byJ9.zx5w-no9EDIHLvoJUxYeSgVrMPGAHgvbaJRH7PxG2pehXoQHwmAZxL3_auYQmwVyQyXa3ijv9_UJwtqGXULU1f9a6kjkSJditFvlfkVGWeczqXP8W6QTAaGt8MtHK7g7ipjQs6r4kf7gcdX-Hgg-zYw26XgNweGVEwOv0HHj4RrvXCi4CEZFw8N2UVwKrHMG4oWVq3UB83A5w4f9J-dmpeS-J7qToC3OFna9TkAQ80xNsXzBnZudUoti3QX1dwPJ5fDJRVUJUGKJNaC8-S_wQDBGXktkp0AotQvlEc0pp_BfoN8QxgEfe0WFGh1NGSh80-9APriM3ocWqrMwSFr0Xw";
        
        const string GET_USERS_URL = "https://localhost:5001/users";
        const string GET_CHATS_URL = "https://localhost:5001/chats";

        /// <summary>
        /// Поиск пользователей
        /// </summary>
        public List<User> SearchUsers { get; set; }

        /// <summary>
        /// Авторизованный пользователь
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Чаты
        /// </summary>
        public List<Chat> Chats { get; set; }

        /// <summary>
        /// Сообщения чата
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChatPageViewModel()
        {
            // TODO: Убрать заглушку

            Task.Run(InitAllInstances).Wait();
        }

        private async Task InitAllInstances()
        {
            User = new User
            {
                Id = Guid.Empty,
                Name = "Дмитрий Тришин",
                Email = "daltrishin@sberbank.ru"
            };

            Chats = await GetChatsAsync();

            Messages = new List<Message> { };
        }

        private async Task<List<Chat>> GetChatsAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
            
            var response = await client.GetAsync(GET_CHATS_URL);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JArray.Parse(responseString);

            var chats = new List<Chat>();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    chats = jsonResponse.ToObject<List<Chat>>();
                    break;
            }

            return chats;
        }
    }
}