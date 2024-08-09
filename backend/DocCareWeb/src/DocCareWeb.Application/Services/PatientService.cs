using AutoMapper;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;

namespace DocCareWeb.Application.Services
{
    public class PatientService : GenericService<Patient, PatientCreateDto, PatientUpdateDto, PatientListDto>, IPatientService
    {
        public PatientService(
            IGenericRepository<Patient> patientRepository,
            IValidator<PatientCreateDto> createValidator,
            IValidator<PatientUpdateDto> updateValidator,
            IMapper mapper,
            INotificator notificator)
            : base(patientRepository, createValidator, updateValidator, mapper, notificator)
        {
        }
    }
}
