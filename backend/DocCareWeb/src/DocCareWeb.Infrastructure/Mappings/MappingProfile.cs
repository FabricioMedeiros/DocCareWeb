using AutoMapper;
using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Dtos.User;
using DocCareWeb.Domain.Entities;


namespace DocCareWeb.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Address mappings
            CreateMap<Address, AddressBaseDto>().ReverseMap();
            CreateMap<Address, AddressCreateDto>().ReverseMap();
            CreateMap<Address, AddressUpdateDto>().ReverseMap();
            CreateMap<Address, AddressListDto>().ReverseMap();

            // Appointment mappings
            CreateMap<Appointment, AppointmentBaseDto>().ReverseMap();
            CreateMap<Appointment, AppointmentCreateDto>();
            CreateMap<AppointmentCreateDto, Appointment>()
               .ForMember(dest => dest.AppointmentDate,
                          opt => opt.MapFrom(src => DateTime.Parse(src.AppointmentDate).Date))
               .ForMember(dest => dest.AppointmentTime,
                          opt => opt.MapFrom(src => TimeSpan.Parse(src.AppointmentTime)));
            CreateMap<Appointment, AppointmentUpdateDto>();
            CreateMap<AppointmentUpdateDto, Appointment>()
               .ForMember(dest => dest.AppointmentDate,
                          opt => opt.MapFrom(src => DateTime.Parse(src.AppointmentDate).Date))
               .ForMember(dest => dest.AppointmentTime,
                          opt => opt.MapFrom(src => TimeSpan.Parse(src.AppointmentTime)));
            CreateMap<Appointment, AppointmentListDto>().ReverseMap();

            // Doctor mappings
            CreateMap<Doctor, DoctorBaseDto>().ReverseMap();
            CreateMap<Doctor, DoctorCreateDto>().ReverseMap();
            CreateMap<Doctor, DoctorUpdateDto>().ReverseMap();
            CreateMap<Doctor, DoctorListDto>().ReverseMap();
            CreateMap<Doctor, DoctorBasicInfoDto>().ReverseMap();

            // Patient mappings
            CreateMap<Patient, PatientBaseDto>().ReverseMap();
            CreateMap<Patient, PatientCreateDto>().ReverseMap();
            CreateMap<Patient, PatientUpdateDto>().ReverseMap();
            CreateMap<Patient, PatientListDto>().ReverseMap();
            CreateMap<Patient, PatientBasicInfoDto>().ReverseMap();

            // HealthPlan mappings
            CreateMap<HealthPlan, HealthPlanBaseDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanCreateDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanUpdateDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanListDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanBasicInfoDto>().ReverseMap();

            // Specialty mappings
            CreateMap<Specialty, SpecialtyBaseDto>().ReverseMap();
            CreateMap<Specialty, SpecialtyCreateDto>().ReverseMap();
            CreateMap<Specialty, SpecialtyUpdateDto>().ReverseMap();
            CreateMap<Specialty, SpecialtyListDto>().ReverseMap();

            // UserRegister mappings
            CreateMap<UserRegisterDto, ApplicationUser>().ReverseMap();

            //Login mappings
            CreateMap<LoginDto, ApplicationUser>().ReverseMap();

            // Custom converters for TimeSpan
            CreateMap<TimeSpan, string>().ConvertUsing(src => src.ToString(@"hh\:mm"));
            CreateMap<string, TimeSpan>().ConvertUsing(src => TimeSpan.Parse(src));

            // Custom converters for DateTime
            CreateMap<DateTime, string>().ConvertUsing(src => src.ToString("yyyy-MM-dd"));
            CreateMap<string, DateTime>().ConvertUsing(src => DateTime.Parse(src));
        }
    }
}
