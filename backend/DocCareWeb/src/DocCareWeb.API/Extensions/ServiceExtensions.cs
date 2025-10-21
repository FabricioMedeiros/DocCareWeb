using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Services;

namespace DocCareWeb.API.Extensions;

public static class ServiceExtensions
{ 
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));

        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IHealthPlanService, HealthPlanService>();        
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();        
        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }    
}
