using System;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Bll.Common.Response;
using Diploma.Bll.Services.Access;
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
        private readonly IAccessManager _accessManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="accessManager">Сервис доступа</param>
        public UserService(DatabaseContext context, IAccessManager accessManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="username">Фильтрация по никнейму</param>
        /// <returns></returns>
        public async Task<UserResponse[]> GetUsersAsync(string username)
        {
            var query = _context.Users.Where(x => x.Id != _accessManager.UserId);

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(x => x.Username.ToLower() == username.ToLower());
            }

            return await query
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Name = x.Username,
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
                    Name = x.Username,
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