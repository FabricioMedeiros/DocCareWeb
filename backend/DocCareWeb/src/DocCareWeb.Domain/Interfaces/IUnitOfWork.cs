namespace DocCareWeb.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        IDoctorRepository Doctors { get; }
        IHealthPlanRepository HealthPlans { get; }
        IPatientRepository Patients { get; }
        ISpecialtyRepository Specialties { get; }
        Task<int> CommitAsync();
    }
}
