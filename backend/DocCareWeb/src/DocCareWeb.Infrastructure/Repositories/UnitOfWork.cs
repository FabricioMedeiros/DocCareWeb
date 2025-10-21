using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IAppointmentRepository Appointments { get; }
        public IAppointmentItemRepository AppointmentsItem { get; }
        public IDoctorRepository Doctors { get; }
        public IHealthPlanRepository HealthPlans { get; }
        public IHealthPlanItemRepository HealthPlansItem { get; }
        public IPatientRepository Patients { get; }
        public IServiceRepository Services { get; }
        public ISpecialtyRepository Specialties { get; }

        public UnitOfWork(ApplicationDbContext context,
            IAppointmentRepository appointments,
            IAppointmentItemRepository appointmentItems,
            IDoctorRepository doctors,
            IHealthPlanRepository healthPlans,
            IHealthPlanItemRepository healthPlanItem,
            IPatientRepository patients,
            IServiceRepository services,
            ISpecialtyRepository specialties
            )
        {
            _context = context;
            Appointments = appointments;
            AppointmentsItem = appointmentItems;
            Doctors = doctors;
            HealthPlans = healthPlans;
            HealthPlansItem = healthPlanItem;
            Patients = patients;
            Services = services;
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
