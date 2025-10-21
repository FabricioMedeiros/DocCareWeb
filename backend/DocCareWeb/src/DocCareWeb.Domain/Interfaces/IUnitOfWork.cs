namespace DocCareWeb.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        IAppointmentItemRepository AppointmentsItem { get; }
        IDoctorRepository Doctors { get; }
        IHealthPlanRepository HealthPlans { get; }
        IHealthPlanItemRepository HealthPlansItem { get; }
        IPatientRepository Patients { get; }
        IServiceRepository Services { get; }
        ISpecialtyRepository Specialties { get; }
        Task<int> CommitAsync();
    }
}
