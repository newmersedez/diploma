using System;
using System.Linq;
using System.Text.RegularExpressions;
using Diploma.Bll.Services.Authorization.Request;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Diploma.Bll.Services.Authorization.Validation
{
    /// <summary>
    /// Валидатор запроса авторизации
    /// </summary>
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthRequestValidator(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var passwordMinimalLength = _configuration.GetValue<int>("Validation:PasswordMinimalLength");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Поле 'Email' обязательно для заполнения")
                .Must(HasValidEmailFormat).WithMessage(x => $"{x.Email} не является электронной почтой");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле 'Пароль' обязательно для заполнения")
                .MinimumLength(passwordMinimalLength).WithMessage("Пароль должен иметь длину не менее 8 символов")
                .Must(HasAtLeastOneNumber).WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Must(HasAtLeastOneCapital).WithMessage("Пароль должен содержать хотя бы одну заглавную букву");
        }

        /// <summary>
        /// Проверка формата Email
        /// </summary>
        /// <param name="email">Электронная почта</param>
        /// <returns></returns>
        private bool HasValidEmailFormat(string email)
        {
            var regex = new Regex(_configuration.GetValue<string>("Validation:EmailFormat"));
            var result = regex.Match(email);
            return result.Success;
        }
        
        /// <summary>
        /// Проверка цифр в пароле
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        private bool HasAtLeastOneNumber(string password)
        {
            return password.Any(char.IsDigit);
        }
        
        /// <summary>
        /// Проверка заглавных букв в пароле
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        private bool HasAtLeastOneCapital(string password)
        {
            return password.Any(char.IsUpper);
        }
    }
}