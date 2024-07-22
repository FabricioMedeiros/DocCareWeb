using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(ApplicationDbContext context) : base(context) { }
    }
}
