using System;
using System.Threading.Tasks;
using Diploma.Bll.Services.Files.Request;

namespace Diploma.Bll.Services.Files
{
    /// <summary>
    /// Сервис управления файлами;
    /// </summary>
    public interface IFilesService
    {
        Task<Guid> CreateFileAsync(CreateFileRequest request);
    }
}