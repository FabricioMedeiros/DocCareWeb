using DocCareWeb.API.Filters;

namespace DocCareWeb.API.Extensions
{
    public static class ControllersExtension
    {
        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();

                options.ModelBinderProviders.Insert(0, new FilterBinderProvider());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
        }
    }
}
