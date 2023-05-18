using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Files.Request;
using Diploma.Persistence;
using Diploma.Persistence.Models.Entities;

namespace Diploma.Bll.Services.Files
{
    /// <summary>
    /// Сервис управления файлами
    /// </summary>
    public class FilesService : IFilesService
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public FilesService(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Создать файл
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        public async Task<Guid> CreateFileAsync(CreateFileRequest request)
        {
            var file = new File
            {
                Id = Guid.NewGuid(),
                Folder = request.Folder,
                Name = request.Name,
                ContentType = request.ContentType
            };

            _context.Files.Add(file);

            await _context.SaveChangesAsync();

            return file.Id;
        }
    }
}