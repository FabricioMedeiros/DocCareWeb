using DocCareWeb.Application.Dtos.DashBoard;

namespace DocCareWeb.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync(int? doctorId = null, DateTime? startDate = null, DateTime? endDate = null);
    }
}
