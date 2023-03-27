using System;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Storage.Common.Features.Verifier
{
    /// <summary>
    /// Настройка валидации
    /// </summary>
    public static class ConfigureVerification
    {
        // TODO: посмотреть как в биме
        /// <summary>
        /// Добавить верификацию
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <typeparam name="TContext">Контекст проверки</typeparam>
        /// <returns></returns>
        public static void AddVerification<TContext>(this IServiceCollection services) 
            where TContext : class
        {
            services.AddScoped<IVerifier<TContext>, Verifier<TContext>>();
        }
    }
}