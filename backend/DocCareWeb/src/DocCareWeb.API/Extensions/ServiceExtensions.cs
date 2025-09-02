using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Services;

namespace DocCareWeb.API.Extensions;

public static class ServiceExtensions
{ 
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IHealthPlanService, HealthPlanService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }    
}
