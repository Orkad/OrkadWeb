using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using OrkadWeb.Application.Common.Behaviors;

namespace OrkadWeb.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Ajoute le pattern CQRS du projet (Mediator / Fluent Validation)
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var asm = Assembly.GetExecutingAssembly();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(asm));
            services.AddValidatorsFromAssembly(asm);
            services.AddAutoMapper(asm);
            services.AddSingleton<ITimeProvider, RealTimeProvider>();
        }
    }
}
