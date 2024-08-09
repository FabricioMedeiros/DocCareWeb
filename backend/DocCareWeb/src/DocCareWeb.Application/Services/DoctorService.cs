using AutoMapper;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;

namespace DocCareWeb.Application.Services
{
    public class DoctorService : GenericService<Doctor, DoctorCreateDto, DoctorUpdateDto, DoctorListDto>, IDoctorService
    {
        public DoctorService(
            IGenericRepository<Doctor> repository,
            IValidator<DoctorCreateDto> createValidator,
            IValidator<DoctorUpdateDto> updateValidator,
            IMapper mapper,
            INotificator notificator)
            : base(repository, createValidator, updateValidator, mapper, notificator)
        {
        }
    }
}
