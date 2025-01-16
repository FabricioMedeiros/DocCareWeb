using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Validators;
using FluentValidation;

namespace DocCareWeb.API.Extensions;

public static class ValidatorsExtensions
{
    public static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AddressCreateDto>, AddressCreateValidator>();
        services.AddScoped<IValidator<AddressUpdateDto>, AddressUpdateValidator>();

        services.AddScoped<IValidator<AppointmentCreateDto>, AppointmentCreateValidator>();
        services.AddScoped<IValidator<AppointmentUpdateDto>, AppointmentUpdateValidator>();

        services.AddScoped<IValidator<DoctorCreateDto>, DoctorCreateValidator>();
        services.AddScoped<IValidator<DoctorUpdateDto>, DoctorUpdateValidator>();

        services.AddScoped<IValidator<HealthPlanCreateDto>, HealthPlanCreateValidator>();
        services.AddScoped<IValidator<HealthPlanUpdateDto>, HealthPlanUpdateValidator>();

        services.AddScoped<IValidator<PatientCreateDto>, PatientCreateValidator>();
        services.AddScoped<IValidator<PatientUpdateDto>, PatientUpdateValidator>();

        services.AddScoped<IValidator<SpecialtyCreateDto>, SpecialtyCreateValidator>();
        services.AddScoped<IValidator<SpecialtyUpdateDto>, SpecialtyUpdateValidator>();
    }
}
