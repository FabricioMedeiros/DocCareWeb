using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context) {}

        public override async Task<IEnumerable<Patient?>> GetAllAsync(Expression<Func<Patient, bool>>? filter = null)
        {
            IQueryable<Patient> query = _context.Set<Patient>()
                                               .Include(p => p.Address)
                                               .Include(p => p.HealthPlan);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public override async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Set<Patient>()
                                .Include(p => p.Address)
                                .Include(p => p.HealthPlan)
                                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
