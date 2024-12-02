using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Domain.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        public Task<Dictionary<string, int>> GetAppointmentsByStatusAsync(DateTime date, int? doctorId = null);
        public Task<FinancialSummary> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate, int? doctorId = null);
    }
}
