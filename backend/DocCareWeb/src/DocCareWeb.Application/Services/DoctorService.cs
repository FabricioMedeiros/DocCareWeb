using AutoMapper;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;

namespace DocCareWeb.Application.Services
{
    public class DoctorService : GenericService<Doctor, DoctorCreateDto, DoctorUpdateDto, DoctorListDto>, IDoctorService
    {
        public DoctorService(
            IUnitOfWork uow,
            IMapper mapper,
            INotificator notificator)
            : base(uow, uow.Doctors, mapper, notificator)
        {
        }
    }
}