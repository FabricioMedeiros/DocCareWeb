using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Domain.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        public Task<int> GetTotalPatientsAsync();
    }
}
