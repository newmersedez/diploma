using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Diploma.Client.Core.MVVM.Command;
using Diploma.Client.Core.MVVM.ViewModel;
using Diploma.Client.MVVM.Model;
using Diploma.Client.Network.Request;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Client.MVVM.ViewModel.Main
{
    /// <summary>
    /// Вьюмодель основного окна чата
    /// </summary>
    public sealed class ChatPageViewModel : ViewModelBase
    {
        private static readonly string GetUsersUri = "https://localhost:5001/users";
        
        private static readonly string CreateChatUri = "https://localhost:5001/chats";
        private static readonly string GetChatsUri = "https://localhost:5001/chats";
        private static readonly string DeleteChatUri = "https://localhost:5001/chats/{0}";
        
        private static readonly string GetMessagesUri = "https://localhost:5001/chats/{0}/messages";
        private static readonly string CreateMessageUri = "https://localhost:5001/chats/{0}/messages";
        
        private static readonly string CreateFileUri = "https://localhost:5001/files";
        private static readonly string UploadFileUri = "https://localhost:5001/storage/files/upload";

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
                        SelectedChatVisibility = Visibility.Hidden;
                        RaisePropertiesChanged(nameof(SelectedChatVisibility));
                    }
                }
                else
                {
                    _selectedChat = value;
                    SelectedChatVisibility = Visibility.Visible;
                    RaisePropertiesChanged(nameof(SelectedChatVisibility));
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
        public Visibility SelectedChatVisibility { get; set; }
        
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
        /// Команда для отправки файла
        /// </summary>
        public RelayCommand SendDocumentCommand { get; }

        /// <summary>
        /// Команда для скачивания файла
        /// </summary>
        public RelayCommand DownloadDocumentCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChatPageViewModel()
        {
            SelectedChatVisibility = Visibility.Hidden;
            
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
                _ => Task.Run(SendTextMessageAsync),
                _ => !string.IsNullOrEmpty(Message) && SelectedChat is not null);

            SendDocumentCommand = new RelayCommand(
                _ => Task.Run(SendDocumentAsync),
                _ => SelectedChat is not null);

            DownloadDocumentCommand = new RelayCommand(_ => Task.Run(DownloadDocumentAsync));
            
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

            var response = await client.GetAsync(GetChatsUri);
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

            var builder = new UriBuilder(GetUsersUri);
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

            var response = await client.GetAsync(string.Format(GetMessagesUri, chatId));
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
            var response = await client.PostAsync(CreateChatUri, content);
            
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
            
            var response = await client.DeleteAsync(string.Format(DeleteChatUri, _selectedChat.Id));
            
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

        private async Task SendTextMessageAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            
            var requestBody = new CreateMessageRequest()
            {
                FileId = null,
                Text = Message
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(string.Format(CreateMessageUri, SelectedChat.Id), content);
            
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

            Message = string.Empty;
            RaisePropertyChanged(nameof(Message));
        }

        private async Task SendDocumentAsync()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false) return;

            var filePath = openFileDialog.FileName;
            
            // загрузка файла в хранилище
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var bytes = await File.ReadAllBytesAsync(filePath);
            var byteArrayContent = new ByteArrayContent(bytes);
            var multipartContent = new MultipartFormDataContent();
            // multipartContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(filePath));
            
            var uploadFileResponse = await client.PostAsync(UploadFileUri, multipartContent);
            var uploadFileResponseString = await uploadFileResponse.Content.ReadAsStringAsync();
            var uploadFileJsonResponse = JObject.Parse(uploadFileResponseString);
            
            // Создание записи в бд
            var createFileRequestBody = uploadFileJsonResponse.ToObject<CreateFileRequest>();
            var createFileRequestContent = new StringContent(JsonConvert.SerializeObject(createFileRequestBody), Encoding.UTF8, "application/json");
            var createFileResponse = await client.PostAsync(CreateFileUri, createFileRequestContent);
            var createFileResponseString = await createFileResponse.Content.ReadAsStringAsync();
            createFileResponseString = createFileResponseString.Trim('"');

            var fileId = Guid.Parse(createFileResponseString);
            
            // Создание комментария
            var createCommentRequestBody = new CreateMessageRequest()
            {
                FileId = fileId,
                Text = "Файл"
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(createCommentRequestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(string.Format(CreateMessageUri, SelectedChat.Id), content);
            
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

            Message = string.Empty;
            RaisePropertyChanged(nameof(Message));
        }

        private async Task DownloadDocumentAsync()
        {
            MessageBox.Show("lalka");
        }
    }
}