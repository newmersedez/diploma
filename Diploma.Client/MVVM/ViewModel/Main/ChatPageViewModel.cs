using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Diploma.Client.Core.MVVM.Command;
using Diploma.Client.Core.MVVM.ViewModel;
using Diploma.Client.MVVM.Model;
using Diploma.Client.Network.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Client.MVVM.ViewModel.Main
{
    /// <summary>
    /// Вьюмодель основного окна чата
    /// </summary>
    public sealed class ChatPageViewModel : ViewModelBase
    {
        private const string TOKEN =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImYyOWU0NTI0LTczYTctNDQ5MS05YmYxLTE5ZjcyNTM4ZTUyZCIsImlzcyI6ImNyeXB0byJ9.zx5w-no9EDIHLvoJUxYeSgVrMPGAHgvbaJRH7PxG2pehXoQHwmAZxL3_auYQmwVyQyXa3ijv9_UJwtqGXULU1f9a6kjkSJditFvlfkVGWeczqXP8W6QTAaGt8MtHK7g7ipjQs6r4kf7gcdX-Hgg-zYw26XgNweGVEwOv0HHj4RrvXCi4CEZFw8N2UVwKrHMG4oWVq3UB83A5w4f9J-dmpeS-J7qToC3OFna9TkAQ80xNsXzBnZudUoti3QX1dwPJ5fDJRVUJUGKJNaC8-S_wQDBGXktkp0AotQvlEc0pp_BfoN8QxgEfe0WFGh1NGSh80-9APriM3ocWqrMwSFr0Xw";

        const string GET_USERS_URL = "https://localhost:5001/users";
        const string GET_CHATS_URL = "https://localhost:5001/chats";
        
        const string CREATE_CHATS_URL = "https://localhost:5001/chats";

        /// <summary>
        /// Поиск пользователей
        /// </summary>
        public List<User> SearchUsers { get; set; }

        /// <summary>
        /// Авторизованный пользователь
        /// </summary>
        public User AuthorizedUser { get; set; }

        /// <summary>
        /// Чаты
        /// </summary>
        public List<Chat> Chats { get; set; }

        /// <summary>
        /// Сообщения чата
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Команда авторизации
        /// </summary>
        public RelayCommand CreateChatCommand { get; }

        /// <summary>
        /// Имя собеседника
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Название чата
        /// </summary>
        public string Chatname { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChatPageViewModel()
        {
            CreateChatCommand = new RelayCommand(
                _ => CreateChatAsync(),
                _ => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Chatname));

            Task.Run(InitAllInstances).Wait();
        }

        private async Task InitAllInstances()
        {
            AuthorizedUser = new User
            {
                Id = Guid.Parse("f29e4524-73a7-4491-9bf1-19f72538e52d"),
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

        private async Task<User> GetUserAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

            var builder = new UriBuilder(GET_USERS_URL);
            builder.Query = $"username={Username}";

            var response = await client.GetAsync(builder.Uri);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JArray.Parse(responseString);

            var foundUsers = jsonResponse.ToObject<List<User>>();
            if (foundUsers.Count == 0) return null;

            return foundUsers.First();
        }

        private async void CreateChatAsync()
        {
            var user = await GetUserAsync();

            if (user is null)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            if (Chats.Any(x => x.Users.Any(y => y.Id == user.Id)))
            {
                MessageBox.Show($"Чат с пользователем {user.Name} уже существует");
                return;
            }
            
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
            
            var requestBody = new CreateChatRequest
            {
                Name = Chatname,
                Users = new[] { user.Id, AuthorizedUser.Id }
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(CREATE_CHATS_URL, content);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Chats = await GetChatsAsync();
                    RaisePropertiesChanged(nameof(Chats));
                    break;
                default:
                    MessageBox.Show("Неизвестная ошибка, попробуйте позже");
                    break;
            }

        }
    }
}