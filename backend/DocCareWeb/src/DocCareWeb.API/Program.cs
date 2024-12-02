using DocCareWeb.API.Filters;
using DocCareWeb.Application.Configurations;
using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Services;
using DocCareWeb.Application.Validators;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using DocCareWeb.Infrastructure.Mappings;
using DocCareWeb.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); 
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Config. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
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
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
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
builder.Services.AddScoped<IDashboardService, DashboardService>();

//Config. JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings!.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

//Config. Swagger
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "DocCareWeb API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor informe o token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

//Config. IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

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

app.UseCors("AllowAnyOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
