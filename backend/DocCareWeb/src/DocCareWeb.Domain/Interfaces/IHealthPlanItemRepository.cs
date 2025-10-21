using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Domain.Interfaces
{
    public interface IHealthPlanItemRepository : IGenericRepository<HealthPlanItem>
    {
        Task<List<HealthPlanItem>> GetByHealthPlanAndServiceIdsAsync(int healthPlanId, List<int> serviceIds);
    }
}
