using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface IServiceService : IGenericService<Service, ServiceCreateDto, ServiceUpdateDto, ServiceListDto>
    {
    }
}
