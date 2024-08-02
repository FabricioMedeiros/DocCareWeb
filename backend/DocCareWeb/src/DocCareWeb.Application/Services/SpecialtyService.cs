using AutoMapper;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;

namespace DocCareWeb.Application.Services
{
    public class SpecialtyService : GenericService<Specialty, SpecialtyCreateDto, SpecialtyUpdateDto, SpecialtyBaseDto>, ISpecialtyService
    {
        public SpecialtyService(
            IGenericRepository<Specialty> specialtyRepository,
            IValidator<SpecialtyCreateDto> createValidator,
            IValidator<SpecialtyUpdateDto> updateValidator,
            IMapper mapper,
            INotificator notificator)
            : base(specialtyRepository, createValidator, updateValidator, mapper, notificator)
        {
        }
    }
}
