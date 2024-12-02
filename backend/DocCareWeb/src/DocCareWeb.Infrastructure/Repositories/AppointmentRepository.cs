using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Enums;
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

        public async Task<Dictionary<string, int>> GetAppointmentsByStatusAsync(DateTime date, int? doctorId = null)
        {
            var query = _context.Set<Appointment>()
                .Where(a => a.AppointmentDate.Date == date.Date);

            if (doctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }

            return await query
                .GroupBy(a => a.Status)
                .ToDictionaryAsync(g => g.Key.ToString(), g => g.Count());
        }

        public async Task<FinancialSummary> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate, int? doctorId = null)
        {
            var receivedQuery = _context.Set<Appointment>()
                .Where(a => a.Status == AppointmentStatus.Completed && a.AppointmentDate.Date >= startDate.Date && a.AppointmentDate.Date <= endDate.Date);

            var pendingQuery = _context.Set<Appointment>()
                .Where(a => a.Status == AppointmentStatus.Confirmed && a.AppointmentDate.Date >= startDate.Date && a.AppointmentDate.Date <= endDate.Date);

            if (doctorId.HasValue)
            {
                receivedQuery = receivedQuery.Where(a => a.DoctorId == doctorId.Value);
                pendingQuery = pendingQuery.Where(a => a.DoctorId == doctorId.Value);
            }

            var totalReceived = await receivedQuery.SumAsync(a => a.Cost);
            var totalPending = await pendingQuery.SumAsync(a => a.Cost);

            return new FinancialSummary
            {
                TotalReceived = totalReceived,
                TotalPending = totalPending
            };
        }

    }
}
