using AutoMapper;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;

namespace DocCareWeb.Application.Services
{
    public class SpecialtyService : GenericService<Specialty, SpecialtyCreateDto, SpecialtyUpdateDto, SpecialtyListDto>, ISpecialtyService
    {
        public SpecialtyService(
            IUnitOfWork uow,
            IMapper mapper,
            INotificator notificator)
            : base(uow, uow.Specialties, mapper, notificator)
        {
        }
    }
}
