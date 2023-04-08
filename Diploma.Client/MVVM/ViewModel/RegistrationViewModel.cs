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
    /// View-model регистрации
    /// </summary>
    public class RegistrationViewModel : ViewModelBase
    {
        private const string REGISTER_URL = "https://localhost:5001/auth/register";

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
        /// Команда регистрации
        /// </summary>
        public RelayCommand RegisterCommand { get; }

        public RegistrationViewModel()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Error = string.Empty;
            
            RegisterCommand = new RelayCommand(
                _ => RegisterUserAsync(),
                _ => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password));
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
            var response = await client.PostAsync(REGISTER_URL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Error = jsonResponse.Value<string>("token");
                    break;
                case HttpStatusCode.Conflict:
                    Error = "Пользователь с таким email уже существует";
                    break;
                case HttpStatusCode.BadRequest:
                    Error = "Ошибка";
                    break;
                default:
                    Error = "Неизвестная ошибка, попробуйте позже";
                    break;
            }

            RaisePropertyChanged(nameof(Error));
        }
    }
}