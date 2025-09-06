using AutoMapper;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Services;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;

public class PatientService : GenericService<Patient, PatientCreateDto, PatientUpdateDto, PatientListDto>, IPatientService
{
    public PatientService(
        IUnitOfWork uow,
        IMapper mapper,
        INotificator notificator)
        : base(uow, uow.Patients, mapper, notificator)
    {
    }
}
