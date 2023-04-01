using System;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Bll.Services.Authorization.Response;
using Diploma.Bll.Services.Users.Response;
using Diploma.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Bll.Services.Users
{
    /// <summary>
    /// Сервис пользователей
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public UserService(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<UserResponse[]> GetUsersAsync()
        {
            return await _context.Users
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Email = x.Email
                })
                .OrderBy(x => x.Email)
                .ToArrayAsync();
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        public async Task<UserResponse> GetUserAsync(Guid userId)
        {
            // TODO: Верификация существования пользователя

            return await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Email = x.Email,
                    PublicKey = new PublicKeyInfo
                    {
                        X = x.PublicKey.X,
                        Y = x.PublicKey.Y
                    }
                })
                .FirstOrDefaultAsync();
        }
    }
}