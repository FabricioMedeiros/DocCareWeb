using DocCareWeb.Application.Configurations;
using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Services;
using DocCareWeb.Application.Validators;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using DocCareWeb.Infrastructure.Mappings;
using DocCareWeb.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Config. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//Config. DBContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Config. Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Config. Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<IHealthPlanRepository, HealthPlanRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

//Config. Services
builder.Services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IHealthPlanService, HealthPlanService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();


//Config. Notifications
builder.Services.AddScoped(typeof(INotificator), typeof(Notificator));

//Config. Validators
builder.Services.AddScoped<IValidator<AddressCreateDto>, AddressCreateValidator>();
builder.Services.AddScoped<IValidator<AddressUpdateDto>, AddressUpdateValidator>();

builder.Services.AddScoped<IValidator<AppointmentCreateDto>, AppointmentCreateValidator>();
builder.Services.AddScoped<IValidator<AppointmentUpdateDto>, AppointmentUpdateValidator>();

builder.Services.AddScoped<IValidator<DoctorCreateDto>, DoctorCreateValidator>();
builder.Services.AddScoped<IValidator<DoctorUpdateDto>, DoctorUpdateValidator>();

builder.Services.AddScoped<IValidator<HealthPlanCreateDto>, HealthPlanCreateValidator>();
builder.Services.AddScoped<IValidator<HealthPlanUpdateDto>, HealthPlanUpdateValidator>();

builder.Services.AddScoped<IValidator<PatientCreateDto>, PatientCreateValidator>();
builder.Services.AddScoped<IValidator<PatientUpdateDto>, PatientUpdateValidator>();

builder.Services.AddScoped<IValidator<SpecialtyCreateDto>, SpecialtyCreateValidator>();
builder.Services.AddScoped<IValidator<SpecialtyUpdateDto>, SpecialtyUpdateValidator>();

// Config. AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
