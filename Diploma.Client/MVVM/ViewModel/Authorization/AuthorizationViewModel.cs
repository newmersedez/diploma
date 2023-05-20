using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Diploma.Client.Core.MVVM.Command;
using Diploma.Client.Core.MVVM.ViewModel;
using Diploma.Client.MVVM.Model;
using Diploma.Client.MVVM.View.Main;
using Diploma.Client.MVVM.View.Main.Pages;
using Diploma.Client.MVVM.ViewModel.Main;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Client.MVVM.ViewModel.Authorization
{
    /// <summary>
    /// Вьюмодель авторизации
    /// </summary>
    public class AuthorizationViewModel : ViewModelBase
    {
        private const string LOGIN_URL = "https://localhost:5001/auth/login";
        private const string REGISTRATION_URL = "https://localhost:5001/auth/register";

        public string Token { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Ошибка авторизации
        /// </summary>
        public string Error { get; set; }
        
        /// <summary>
        /// Команда авторизации
        /// </summary>
        public RelayCommand LoginCommand { get; }
        
        /// <summary>
        /// Команда регистрации
        /// </summary>
        public RelayCommand RegisterCommand { get; }

        public AuthorizationViewModel()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Error = string.Empty;
            
            LoginCommand = new RelayCommand(
                _ => LoginUserAsync(),
                _ => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password));
            
            RegisterCommand = new RelayCommand(
                _ => RegisterUserAsync(),
                _ => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password));
        }
        
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        private async void LoginUserAsync()
        {
            var client = new HttpClient();
            
            var requestBody = new Dictionary<string, string>
            {
                { "Email", Email },
                { "Password", Password }
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(LOGIN_URL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = string.IsNullOrEmpty(responseString) ? new JObject() : JObject.Parse(responseString);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Token = jsonResponse.Value<string>("token");
                    OpenMainWindow();
                    break;
                case HttpStatusCode.BadRequest:
                    Error = "  • Неверный email или пароль";
                    break;
                case HttpStatusCode.Conflict:
                    Error = "  • Пользователь не существует";
                    break;
                case HttpStatusCode.Forbidden:
                    Error = "  • Неверный пароль";
                    break;
                default:
                    Error = "  • Неизвестная ошибка, попробуйте позже";
                    break;
            }

            RaisePropertyChanged(nameof(Error));
        }
        
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        private async void RegisterUserAsync()
        {
            var client = new HttpClient();
            
            var requestBody = new Dictionary<string, string>
            {
                { "Name", Name },
                { "Email", Email },
                { "Password", Password }
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(REGISTRATION_URL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = string.IsNullOrEmpty(responseString) ? new JObject() : JObject.Parse(responseString);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Token = jsonResponse.Value<string>("token");
                    OpenMainWindow();
                    break;
                case HttpStatusCode.Conflict:
                    Error = "  • Пользователь с таким email уже существует";
                    break;
                case HttpStatusCode.BadRequest:
                    var error = jsonResponse.GetValue("errors").Children();
                    var passwordErrors = error.Children().Values<string>().Select(x => $"  • {x}");
                    Error = string.Join("\n", passwordErrors);
                    break;
                default:
                    Error = "  • Неизвестная ошибка, попробуйте позже";
                    break;
            }

            RaisePropertyChanged(nameof(Error));
        }
        
        /// <summary>
        /// Открыть основное окно
        /// </summary>
        /// <param name="token">Токен</param>
        private void OpenMainWindow()
        {
            File.Delete("token.json");
            File.WriteAllText("token.json", Token);

            var window = new MainWindow();
            
            Application.Current.MainWindow?.Close();
            Application.Current.MainWindow = window;
            Application.Current.MainWindow.Show();
        }
    }
}