using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Diploma.Client.Core.MVVM.Command;
using Diploma.Client.Core.MVVM.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Client.MVVM.ViewModel
{
    /// <summary>
    /// View-model авторизации
    /// </summary>
    public sealed class AuthViewModel : ViewModelBase
    {
        private const string LOGIN_URL = "https://localhost:5002/auth/login";
        private const string CONTENT_TYPE = "application/json";

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
        /// Команда логина
        /// </summary>
        public RelayCommand LoginCommand { get; }

        public AuthViewModel()
        {
            Email = string.Empty;
            Password = string.Empty;
            Error = string.Empty;
            
            LoginCommand = new RelayCommand(
                _ => LoginUserAsync(),
                _ => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password));
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
            
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, CONTENT_TYPE);
            var response = await client.PostAsync(LOGIN_URL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Error = jsonResponse.Value<string>("token");
                    break;
                case HttpStatusCode.Forbidden:
                    Error = "Неверный пароль";
                    break;
                case HttpStatusCode.BadRequest:
                    Error = "Неверный email или пароль";
                    break;
            }

            RaisePropertyChanged(nameof(Error));
        }
    }
}