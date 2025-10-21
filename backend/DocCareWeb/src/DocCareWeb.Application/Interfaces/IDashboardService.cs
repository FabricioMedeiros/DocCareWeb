using DocCareWeb.Application.Dtos.DashBoard;

namespace DocCareWeb.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync(DateTime startDate, DateTime endDate, int? doctorId = null);
    }
}
