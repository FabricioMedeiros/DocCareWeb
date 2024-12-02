using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Dtos.DashBoard
{
    public class DashboardDto
    {
        public int TotalPatients { get; set; }
        public required Dictionary<string, int> AppointmentsByStatus { get; set; }
        public required FinancialSummary DailyFinancialSummary { get; set; }
        public required FinancialSummary MonthlyFinancialSummary { get; set; }
    }

}
