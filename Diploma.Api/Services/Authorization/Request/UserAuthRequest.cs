namespace Diploma.Server.Services.Authorization.Request
{
    /// <summary>
    /// Запрос на авторизацию пользователя
    /// </summary>
    public sealed class UserAuthRequest
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