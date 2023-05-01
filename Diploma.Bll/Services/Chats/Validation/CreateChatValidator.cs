using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public CreateChatValidator(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название чата не должно быть пустым");

            RuleFor(x => x.Users)
                .NotEmpty().WithMessage("Выберите собеседника")
                .Must(UserExists).WithMessage("Пользователь не существует");
        }
        
        /// <summary>
        /// Пользователь существует
        /// </summary>
        /// <param name="userIds">Идентификаторы пользователей</param>
        /// <returns></returns>
        private bool UserExists(List<Guid> userIds)
        {
            return _context.Users.Any(x => userIds.All(userId => x.Id != userId));
        }
    }
}