using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IAppointmentRepository Appointments { get; }
        public IDoctorRepository Doctors { get; }
        public IHealthPlanRepository HealthPlans { get; }
        public IPatientRepository Patients { get; }
        public ISpecialtyRepository Specialties { get; }

        public UnitOfWork(ApplicationDbContext context,
            IAppointmentRepository appointments,
            IDoctorRepository doctors,
            IHealthPlanRepository healthPlans,
            IPatientRepository patients,
            ISpecialtyRepository specialties
            )
        {
            _context = context;
            Appointments = appointments;
            Doctors = doctors;
            HealthPlans = healthPlans;
            Patients = patients;
            Specialties = specialties;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
