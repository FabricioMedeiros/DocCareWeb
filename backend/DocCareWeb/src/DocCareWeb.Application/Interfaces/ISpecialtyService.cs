using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface ISpecialtyService : IGenericService<Specialty, SpecialtyCreateDto, SpecialtyUpdateDto, SpecialtyBaseDto>
    {
    }
}
