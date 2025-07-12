//using HerbalHub.CommonMethods;
using HerbalHub.Helpers;
using HerbalHub.Interfaces;
using HerbalHub.Services;

namespace HerbalHub.ServiceRegistration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register all services here
            services.AddScoped<IUser, UserService>();
            services.AddScoped<JwtService>();
            services.AddScoped<EmailService>();
          //  services.AddScoped<StoredProcExecutor>();
            return services;
        }
    }
}
