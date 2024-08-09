using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface IHealthPlanService : IGenericService<HealthPlan, HealthPlanCreateDto, HealthPlanUpdateDto, HealthPlanListDto>
    {

    }
}
