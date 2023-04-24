using System;
using System.Linq;
using System.Text.RegularExpressions;
using Diploma.Bll.Services.Authorization.Request.Diploma.Bll.Services.Authorization.Request;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Diploma.Bll.Services.Authorization.Validation
{
    /// <summary>
    /// Валидатор запроса авторизации
    /// </summary>
    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        public LoginUserValidator(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            RuleFor(x => x.Email)
                .Must(HasValidEmailFormat).WithMessage(x => $"'{x.Email}' не является электронной почтой");
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
    }
}