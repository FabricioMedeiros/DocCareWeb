using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface IPatientService : IGenericService<Patient, PatientCreateDto, PatientUpdateDto, PatientBaseDto>
    {
    }
}
