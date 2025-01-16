using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Repositories;

namespace DocCareWeb.API.Extensions;

public static class RepositoriesExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IHealthPlanRepository, HealthPlanRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
    }
}
