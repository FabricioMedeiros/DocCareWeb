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

            // Doctor mappings
            CreateMap<Doctor, DoctorBaseDto>().ReverseMap();
            CreateMap<Doctor, DoctorCreateDto>().ReverseMap();
            CreateMap<Doctor, DoctorUpdateDto>().ReverseMap();
            CreateMap<Doctor, DoctorListDto>().ReverseMap();

            // Appointment mappings
            CreateMap<Appointment, AppointmentBaseDto>().ReverseMap();
            CreateMap<Appointment, AppointmentCreateDto>().ReverseMap();
            CreateMap<Appointment, AppointmentUpdateDto>().ReverseMap();
            CreateMap<Appointment, AppointmentListDto>().ReverseMap();

            // Patient mappings
            CreateMap<Patient, PatientBaseDto>().ReverseMap();
            CreateMap<Patient, PatientCreateDto>().ReverseMap();
            CreateMap<Patient, PatientUpdateDto>().ReverseMap();
            CreateMap<Patient, PatientListDto>().ReverseMap();

            // HealthPlan mappings
            CreateMap<HealthPlan, HealthPlanBaseDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanCreateDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanUpdateDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanListDto>().ReverseMap();

            // Specialty mappings
            CreateMap<Specialty, SpecialtyBaseDto>().ReverseMap();
            CreateMap<Specialty, SpecialtyCreateDto>().ReverseMap();
            CreateMap<Specialty, SpecialtyUpdateDto>().ReverseMap();
            CreateMap<Specialty, SpecialtyListDto>().ReverseMap();

            // UserRegister mappings
            CreateMap<UserRegisterDto, ApplicationUser>().ReverseMap();

            //Login mappings
            CreateMap<LoginDto, ApplicationUser>().ReverseMap();
        }
    }
}
