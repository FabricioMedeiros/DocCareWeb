using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)  { }
        public override async Task<IEnumerable<Appointment?>> GetAllAsync(Expression<Func<Appointment, bool>>? filter = null)
        {
            IQueryable<Appointment> query = _context.Set<Appointment>()
                                               .Include(a => a.Doctor)
                                               .Include(a => a.Patient)
                                               .Include(a => a.HealthPlan);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public override async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Set<Appointment>()
                                .Include(a => a.Doctor)
                                .Include(a => a.Patient)
                                .Include(a => a.HealthPlan)
                                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
