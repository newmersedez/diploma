using System;
using System.Linq;
using System.Text.RegularExpressions;
using Diploma.Bll.Services.Authorization.Request;
using Diploma.Persistence;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Diploma.Bll.Services.Authorization.Validation
{
    /// <summary>
    /// Валидатор запроса регистрации
    /// </summary>
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="context">Контекст БД</param>
        public RegisterUserValidator(IConfiguration configuration, DatabaseContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            var passwordMinimalLength = _configuration.GetValue<int>("Validation:PasswordMinimalLength");

            RuleFor(x => x.Name)
                .Must(NameUnique).WithMessage("Имя должно быть уникальным");

            RuleFor(x => x.Email)
                .Must(HasValidEmailFormat).WithMessage(x => $"'{x.Email}' не является электронной почтой");

            RuleFor(x => x.Password)
                .MinimumLength(passwordMinimalLength).WithMessage("Пароль должен иметь длину не менее 8 символов")
                .Must(HasAtLeastOneNumber).WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Must(HasAtLeastOneCapital).WithMessage("Пароль должен содержать хотя бы одну заглавную букву");
        }

        /// <summary>
        /// Проверка формата имени
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns></returns>
        private bool NameUnique(string name)
        {
            return !_context.Users.Any(x => x.Username.ToLower() == name.Trim().ToLower());
        }
        
        /// <summary>
        /// Проверка формата Email
        /// </summary>
        /// <param name="email">Электронная почта</param>
        /// <returns></returns>
        private bool HasValidEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            
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
            if (string.IsNullOrEmpty(password)) return false;

            return password.Any(char.IsDigit);
        }
        
        /// <summary>
        /// Проверка заглавных букв в пароле
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        private bool HasAtLeastOneCapital(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            return password.Any(char.IsUpper);
        }
    }
}