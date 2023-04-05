using System;
using System.Linq;
using Diploma.Bll.Services.Messages.Request;
using Diploma.Persistence;
using FluentValidation;

namespace Diploma.Bll.Services.Messages.Validation
{
    /// <summary>
    /// Валидатор создания сообщения
    /// </summary>
    public class CreateMessageValidator : AbstractValidator<CreateMessageRequest>
    {
        private readonly DatabaseContext _context;
        
        public CreateMessageValidator(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Текст сообщения обязательный");

            RuleFor(x => x.AttachmentId)
                .Must(AttachmentExists).WithMessage("Файл не существует");
        }

        /// <summary>
        /// Проверка существования файла
        /// </summary>
        /// <param name="attachmentId">Идентификатор файла</param>
        /// <returns></returns>
        private bool AttachmentExists(Guid? attachmentId)
        {
            if (attachmentId == null) return true;

            var isExists = _context.Attachments.Any(x => x.Id == attachmentId);

            return isExists;
        }
    }
}