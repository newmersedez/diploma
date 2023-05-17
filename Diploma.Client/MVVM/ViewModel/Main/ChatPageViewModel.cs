using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Diploma.Client.Core.MVVM.Command;
using Diploma.Client.Core.MVVM.ViewModel;
using Diploma.Client.MVVM.Model;
using Diploma.Client.MVVM.ViewModel.Authorization;
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
        const string GET_USERS_URL = "https://localhost:5001/users";
        const string GET_CHATS_URL = "https://localhost:5001/chats";
        const string GET_MESSAGES_URL = "https://localhost:5001/chats/{0}/messages";
        
        const string CREATE_CHAT_URL = "https://localhost:5001/chats";
        const string DELETE_CHAT_URL = "https://localhost:5001/chats/{0}";
        const string CREATE_MESSAGE_URL = "https://localhost:5001/chats/{0}/messages";

        private Chat _selectedChat;

        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Авторизованный пользователь
        /// </summary>
        public User AuthorizedUser { get; set; }

        /// <summary>
        /// Чаты
        /// </summary>
        public List<Chat> Chats { get; set; }
        
        /// <summary>
        /// Выбранный чат
        /// </summary>
        public Chat SelectedChat
        {
            get => _selectedChat;
            
            set
            {
                if (value is null)
                {
                    if (Chats.Count == 0)
                    {
                        _selectedChat = null;
                        Visibility = Visibility.Hidden;
                        RaisePropertiesChanged(nameof(Visibility));
                    }
                }
                else
                {
                    _selectedChat = value;
                    Visibility = Visibility.Visible;
                    RaisePropertiesChanged(nameof(Visibility));
                    Messages = Task.Run(() => GetMessagesAsync(_selectedChat.Id)).Result;
                    RaisePropertiesChanged(nameof(SelectedChat));   
                }
            }
        }

        /// <summary>
        /// Сообщения чата
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Команда создания чата
        /// </summary>
        public RelayCommand CreateChatCommand { get; }

        /// <summary>
        /// Команда удаления чата
        /// </summary>
        public RelayCommand DeleteChatCommand { get; }

        /// <summary>
        /// Команда для отправки сообщения
        /// </summary>
        public RelayCommand SendMessageCommand { get; }

        /// <summary>
        /// Имя собеседника
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Название чата
        /// </summary>
        public string Chatname { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Видимость чата
        /// </summary>
        public Visibility Visibility { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChatPageViewModel()
        {
            Visibility = Visibility.Hidden;
            
            if (!File.Exists("token.json"))
            {
                throw new ArgumentNullException($"token.json");
            }

            Token = File.ReadAllText("token.json");
            File.Delete("token.json");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(Token);

            AuthorizedUser = new User
            {
                Id = Guid.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")!.Value),
                Email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")!.Value,
                Name = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")!.Value,
                PublicKey = new PublicKey
                {
                    X = jwtToken.Claims.FirstOrDefault(x => x.Type == "publicX")!.Value,
                    Y = jwtToken.Claims.FirstOrDefault(x => x.Type == "publicY")!.Value
                }
            };
            
            CreateChatCommand = new RelayCommand(
                _ => Task.Run(CreateChatAsync),
                _ => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Chatname));

            DeleteChatCommand = new RelayCommand(
                _ => Task.Run(DeleteChatAsync));

            SendMessageCommand = new RelayCommand(
                _ => Task.Run(SendMessageAsync),
                _ => !string.IsNullOrEmpty(Message) && SelectedChat is not null);
            
            Task.Run(LoadContentAsync);

            Task.Run(UpdatesFromServer);
        }

        private async Task LoadContentAsync()
        {
            Chats = await GetChatsAsync();
            RaisePropertyChanged(nameof(Chats));

        }

        private async Task UpdatesFromServer()
        {
            while (true)
            {
                Chats = await GetChatsAsync();
                RaisePropertyChanged(nameof(Chats));

                if (SelectedChat is not null)
                {
                    Messages = await GetMessagesAsync(SelectedChat.Id);
                    RaisePropertiesChanged(nameof(Message));
                }

                await Task.Delay(1000);
            }
        }
        
        private async Task<List<Chat>> GetChatsAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

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

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var builder = new UriBuilder(GET_USERS_URL);
            builder.Query = $"username={Username}";

            var response = await client.GetAsync(builder.Uri);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JArray.Parse(responseString);

            var foundUsers = jsonResponse.ToObject<List<User>>();
            if (foundUsers.Count == 0) return null;

            return foundUsers.First();
        }

        private async Task<List<Message>> GetMessagesAsync(Guid chatId)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await client.GetAsync(string.Format(GET_MESSAGES_URL, chatId));
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JArray.Parse(responseString);

            var messages = new List<Message>();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    messages = jsonResponse.ToObject<List<Message>>();
                    RaisePropertiesChanged(nameof(Messages));
                    break;
            }

            return messages;
        }

        private async Task CreateChatAsync()
        {
            if (Username == AuthorizedUser.Name)
            {
                MessageBox.Show("Нельзя создать чат с самим собой");
                return;
            }
            
            var user = await GetUserAsync();

            if (user is null)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }
            
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            
            var requestBody = new CreateChatRequest
            {
                Name = Chatname,
                UserId = user.Id
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(CREATE_CHAT_URL, content);
            
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

        private async Task DeleteChatAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            
            var response = await client.DeleteAsync(string.Format(DELETE_CHAT_URL, _selectedChat.Id));
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Chats = await GetChatsAsync();
                    RaisePropertiesChanged(nameof(Chats));
                    // SelectedChat = Chats.FirstOrDefault();
                    // RaisePropertiesChanged(nameof(SelectedChat));
                    break;
                default:
                    MessageBox.Show("Неизвестная ошибка, попробуйте позже");
                    break;
            }
        }

        private async Task SendMessageAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            
            var requestBody = new CreateMessageRequest()
            {
                AttachmentId = null,
                Text = Message
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(string.Format(CREATE_MESSAGE_URL, SelectedChat.Id), content);
            
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