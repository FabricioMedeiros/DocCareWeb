using DocCareWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocCareWeb.API.Extensions;

public static class DatabaseExtensions
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}