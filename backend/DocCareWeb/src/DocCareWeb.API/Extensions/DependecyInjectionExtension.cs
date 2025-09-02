using DocCareWeb.Application.Notifications;
using DocCareWeb.Infrastructure.Mappings;

namespace DocCareWeb.API.Extensions
{
    public static class DependecyInjectionExtension
    {
        public static void ConfigureHttpContextAccessor(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        public static void ConfigureNotifications(this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
