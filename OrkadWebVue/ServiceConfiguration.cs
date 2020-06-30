using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Services.Authentication;

namespace OrkadWebVue
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            return services;
        }
    }
}
