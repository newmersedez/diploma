using System;
using System.Collections.Generic;
using System.Linq;
using Diploma.Bll.Services.Access;
using Diploma.Bll.Services.Chats.Request;
using Diploma.Persistence;
using FluentValidation;

namespace Diploma.Bll.Services.Chats.Validation
{
    /// <summary>
    /// Валидатор создания чата
    /// </summary>
    public sealed class CreateChatValidator : AbstractValidator<CreateChatRequest>
    {
        private readonly DatabaseContext _context;
        private readonly IAccessManager _accessManager;
        
        public CreateChatValidator(DatabaseContext context, IAccessManager accessManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название чата не должно быть пустым");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Выберите собеседника")
                .Must(UserExists).WithMessage("Пользователь не существует")
                .Must(UserNotHimself).WithMessage("Пользователь не может начать чат с самим собой");
        }

        /// <summary>
        /// Пользователь не начинает чат с самим собой
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool UserNotHimself(Guid userId)
        {
            return userId != _accessManager.UserId;
        }

        /// <summary>
        /// Пользователь существует
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        private bool UserExists(Guid userId)
        {
            return _context.Users.Any(x => x.Id == userId);
        }
    }
}