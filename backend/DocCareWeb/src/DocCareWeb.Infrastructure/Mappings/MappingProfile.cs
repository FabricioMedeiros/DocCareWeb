using AutoMapper;
using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Dtos.AppointmentItem;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Dtos.HealthPlanItem;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Dtos.Service;
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
            CreateMap<AppointmentCreateDto, Appointment>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => DateTime.Parse(src.AppointmentDate).Date))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.EndTime)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<Appointment, AppointmentUpdateDto>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString(@"hh\:mm")))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString(@"hh\:mm")))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
                    src.Items.Select(i => new AppointmentItemUpdateDto
                    {
                        ServiceId = i.ServiceId,
                        SuggestedPrice = i.SuggestedPrice,
                        Price = i.Price
                    }).ToList()));

            CreateMap<AppointmentUpdateDto, Appointment>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => DateTime.Parse(src.AppointmentDate)))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.EndTime)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
                    src.Items.Select(i => new AppointmentItem
                    {
                        ServiceId = i.ServiceId,
                        Price = i.Price
                    }).ToList()))
                .AfterMap((src, dest) => {
                    foreach (var item in dest.Items)
                    {
                        item.AppointmentId = dest.Id;
                    }
                });

            CreateMap<Appointment, AppointmentListDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // AppointmentItems mappings
            CreateMap<AppointmentItem, AppointmentItemBaseDto>().ReverseMap();
            CreateMap<AppointmentItem, AppointmentItemCreateDto>().ReverseMap();
            CreateMap<AppointmentItem, AppointmentItemUpdateDto>().ReverseMap();
            CreateMap<AppointmentItem, AppointmentItemListDto>()
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Service.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.SuggestedPrice, opt => opt.MapFrom(src => src.Service.BasePrice))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));            

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

            CreateMap<HealthPlan, HealthPlanUpdateDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
                    src.Items.Select(i => new HealthPlanItemUpdateDto
                    {
                        ServiceId = i.ServiceId,
                        Price = i.Price
                    }).ToList()));

            CreateMap<HealthPlanUpdateDto, HealthPlan>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
                    src.Items.Select(i => new HealthPlanItem
                    {
                        ServiceId = i.ServiceId,
                        Price = i.Price
                    }).ToList()))
                .AfterMap((src, dest) => {
                    foreach (var item in dest.Items)
                    {
                        item.HealthPlanId = dest.Id;
                    }
                });

            CreateMap<HealthPlan, HealthPlanListDto>().ReverseMap();
            CreateMap<HealthPlan, HealthPlanBasicInfoDto>().ReverseMap();

            //HealthPlanItems mappings
            CreateMap<HealthPlanItem, HealthPlanItemBaseDto>().ReverseMap();
            CreateMap<HealthPlanItem, HealthPlanItemCreateDto>().ReverseMap();
            CreateMap<HealthPlanItem, HealthPlanItemUpdateDto>().ReverseMap();
            CreateMap<HealthPlanItem, HealthPlanItemListDto>()
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Service.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));


            //Service mappings
            CreateMap<Service, ServiceBaseDto>().ReverseMap();
            CreateMap<Service, ServiceCreateDto>().ReverseMap();
            CreateMap<Service, ServiceUpdateDto>().ReverseMap();
            CreateMap<Service, ServiceListDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.IsHealthPlanPrice, opt => opt.Ignore());

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
