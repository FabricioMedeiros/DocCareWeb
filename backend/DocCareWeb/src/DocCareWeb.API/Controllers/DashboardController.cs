using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace DocCareWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : MainController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService, INotificator notificator) : base(notificator)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData(int? doctorId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var data = await _dashboardService.GetDashboardDataAsync(doctorId, startDate, endDate);
            return CustomResponse(data);
        }

    }
}
