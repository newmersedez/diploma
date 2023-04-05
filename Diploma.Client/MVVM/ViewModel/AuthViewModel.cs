using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Diploma.Client.Core.MVVM.Command;
using Diploma.Client.Core.MVVM.ViewModel;
using Newtonsoft.Json;

namespace Diploma.Client.MVVM.ViewModel
{
    /// <summary>
    /// View-model авторизации
    /// </summary>
    public sealed class AuthViewModel : ViewModelBase
    {
        private const string LOGIN_URL = "https://localhost:5002/auth/login";

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Команда логина
        /// </summary>
        public RelayCommand LoginCommand { get; set; }

        public AuthViewModel()
        {
            Email = string.Empty;
            Password = string.Empty;
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
            var requestBodyJson = JsonConvert.SerializeObject( requestBody );
            var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(LOGIN_URL, content);
            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}