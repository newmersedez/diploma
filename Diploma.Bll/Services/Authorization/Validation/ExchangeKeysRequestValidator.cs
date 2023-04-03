using System.Linq;
using Diploma.Bll.Services.Authorization.Request;
using FluentValidation;

namespace Diploma.Bll.Services.Authorization.Validation
{
    /// <summary>
    /// Валидатор запроса обмена ключей
    /// </summary>
    public class ExchangeKeysRequestValidator : AbstractValidator<ExchangeKeysRequest>
    {
        public ExchangeKeysRequestValidator()
        {
            RuleFor(x => x.PublicKey)
                .NotEmpty().WithMessage("Не указан публичный ключ");

            RuleFor(x => x.PublicKey.X)
                .NotEmpty().WithMessage("Не указано значение X")
                .Must(HasIntegerFormat).WithMessage(x => $"Введенное знаение X {x.PublicKey.X} не является числом");
            
            RuleFor(x => x.PublicKey.Y)
                .NotEmpty().WithMessage("Не указано значение Y")
                .Must(HasIntegerFormat).WithMessage(x => $"Введенное знаение Y {x.PublicKey.X} не является числом");
        }

        /// <summary>
        /// Проверка на число
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns></returns>
        private bool HasIntegerFormat(string arg)
        {
            return arg.All(char.IsDigit);
        }
    }
}