using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class HealthPlanRepository : GenericRepository<HealthPlan>, IHealthPlanRepository
    {
        public HealthPlanRepository(ApplicationDbContext context) : base(context) { }
    }
}
