using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Repositories;

namespace DocCareWeb.API.Extensions;

public static class RepositoriesExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IAppointmentItemRepository, AppointmentItemRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IHealthPlanRepository, HealthPlanRepository>();
        services.AddScoped<IHealthPlanItemRepository, HealthPlanItemRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();  

        services.AddScoped<IUnitOfWork, UnitOfWork>();
;    }
}
