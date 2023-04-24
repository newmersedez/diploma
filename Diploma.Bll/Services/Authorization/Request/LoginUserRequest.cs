namespace Diploma.Bll.Services.Authorization.Request
{
    namespace Diploma.Bll.Services.Authorization.Request
    {
        /// <summary>
        /// Запрос на авторизацию пользователя
        /// </summary>
        public sealed class LoginUserRequest
        {
            /// <summary>
            /// Электронный адрес
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Пароль
            /// </summary>
            public string Password { get; set; }
        }
    }
}