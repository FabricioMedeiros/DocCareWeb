using DocCareWeb.Application.Dtos.DashBoard;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Interfaces;

namespace DocCareWeb.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;

        public DashboardService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }

        public async Task<DashboardDto> GetDashboardDataAsync(int? doctorId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var today = DateTime.Today;
            var defaultStartDate = new DateTime(today.Year, today.Month, 1); 
            var defaultEndDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)); 

            startDate ??= defaultStartDate;
            endDate ??= defaultEndDate;

            var totalPatients = await _patientRepository.GetTotalPatientsAsync();

            var appointmentsByStatus = await _appointmentRepository.GetAppointmentsByStatusAsync(today, doctorId);

            var dailyFinancials = await _appointmentRepository.GetFinancialSummaryAsync(today, today, doctorId);

            var financialSummary = await _appointmentRepository.GetFinancialSummaryAsync(
                startDate.Value,
                endDate.Value,
                doctorId
            );

            return new DashboardDto
            {
                TotalPatients = totalPatients,
                AppointmentsByStatus = appointmentsByStatus,
                DailyFinancialSummary = dailyFinancials,
                MonthlyFinancialSummary = financialSummary
            };
        }
    }
}
