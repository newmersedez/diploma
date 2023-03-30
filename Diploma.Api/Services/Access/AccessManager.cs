using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Diploma.Server.Services.AccessManager
{
    /// <summary>
    /// Сервис управления доступом
    /// </summary>
    public sealed class AccessManager : IAccessManager
    {
        public Guid UserId
        {
            get => Guid.Parse("8094b01e-33d6-4c3b-8d60-46de42386c5c");
        }
    }
}