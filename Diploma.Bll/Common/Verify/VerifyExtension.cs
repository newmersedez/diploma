using System;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Bll.Common.Verify
{
    /// <summary>
    /// Расширение для валидации
    /// </summary>
    public static class VerifyExtension
    {
        public static IServiceCollection VerifyContext<TValidateContext>(
            this IServiceCollection services, Action<AddVerify<TValidateContext>> action)
        {
            var addVerify = new AddVerify<TValidateContext>(services);
            action(addVerify);

            services.AddScoped<IVerifier<TValidateContext>, Verifier<TValidateContext>>();

            return services;
        }

        /// <summary>
        /// Регистратор экземпляра проверки
        /// </summary>
        public sealed class AddVerify<TValidateContext>
        {
            private readonly IServiceCollection _services;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="services">Коллекция сервисов</param>
            public AddVerify(IServiceCollection services)
            {
                _services = services ?? throw new ArgumentNullException(nameof(services));
            }

            public AddVerify<TValidateContext> Add<T>() where T : class, IVerify<TValidateContext>
            {
                _services.AddScoped<IVerify<TValidateContext>, T>();

                return this;
            }
        }
    }
}