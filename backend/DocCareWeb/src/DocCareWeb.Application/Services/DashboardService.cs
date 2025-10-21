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

        public async Task<DashboardDto> GetDashboardDataAsync(DateTime startDate, DateTime endDate, int? doctorId = null)
        {
            var today = DateTime.Today;
            
            var totalPatients = await _patientRepository.GetTotalPatientsAsync();

            var appointmentsByStatus = await _appointmentRepository.GetAppointmentsByStatusAsync(startDate, endDate, doctorId);

            var financialSummary = await _appointmentRepository.GetFinancialSummaryAsync(startDate, endDate, doctorId);

            return new DashboardDto
            {
                TotalPatients = totalPatients,
                AppointmentsByStatus = appointmentsByStatus,
                FinancialSummary = financialSummary
            };
        }
    }
}
