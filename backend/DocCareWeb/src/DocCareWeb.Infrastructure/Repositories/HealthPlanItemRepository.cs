using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class HealthPlanItemRepository : GenericRepository<HealthPlanItem>, IHealthPlanItemRepository
    {
        public HealthPlanItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<HealthPlanItem>> GetByHealthPlanAndServiceIdsAsync(int healthPlanId, List<int> serviceIds)
        {
            return await _context.HealthPlanItems
                .Where(h => h.HealthPlanId == healthPlanId && serviceIds.Contains(h.ServiceId))
                .ToListAsync();
        }
    }
}
