using System.Linq;
using Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Response;
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
                .NotEmpty().WithMessage("Не указан публичный ключ")
                .Must(key => HasIntegerFormat(key?.X)).WithMessage("Введенное знаение X не является числом")
                .Must(key => HasIntegerFormat(key?.Y)).WithMessage("Введенное знаение Y не является числом");
        }

        // /// <summary>
        // /// Проверка ключа
        // /// </summary>
        // /// <param name="publicKey">Публичный ключ</param>
        // /// <returns></returns>
        // private bool HasIntegerFormat(PublicKeyInfo publicKey)
        // {
        //     if (publicKey == null) return false;
        //     
        //     return publicKey.
        // }

        /// <summary>
        /// Проверка на число
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns></returns>
        private bool HasIntegerFormat(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return false;
            
            return arg.All(char.IsDigit);
        }
    }
}