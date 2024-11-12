using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<Doctor?>> GetAllAsync(Expression<Func<Doctor, bool>>? filter = null)
        {
            IQueryable<Doctor> query = _context.Set<Doctor>()
                                               .Include(d => d.Specialty);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public override async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Set<Doctor>()
                                 .Include(d => d.Specialty)
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
