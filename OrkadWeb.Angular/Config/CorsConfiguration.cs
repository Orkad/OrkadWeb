using Microsoft.Extensions.DependencyInjection;

namespace OrkadWeb.Angular.Config
{
    public static class CorsConfiguration
    {
        public const string DEFAULT_POLICY = "ORKAD_WEB_POLICY";

        public static IServiceCollection AddOrkadWebCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DEFAULT_POLICY,
                    policy =>
                    {
                        policy
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials();
                    });
            });
            return services;
        }
    }
}
