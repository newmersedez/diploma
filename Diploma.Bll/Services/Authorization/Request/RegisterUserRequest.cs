namespace Diploma.Bll.Services.Authorization.Request
{
    /// <summary>
    /// Запрос на регистрацию пользователя
    /// </summary>
    public sealed class RegisterUserRequest
    {
        /// <summary>
        /// Никнейм
        /// </summary>
        public string Name { get; set; }

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