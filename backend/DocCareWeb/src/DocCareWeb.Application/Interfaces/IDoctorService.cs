using DocCareWeb.Application.Dtos;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface IDoctorService : IGenericService<Doctor, DoctorCreateDto, DoctorUpdateDto, DoctorBaseDto>
    {
    }
}
