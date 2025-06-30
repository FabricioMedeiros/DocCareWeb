using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context) {}

        public async Task<int> GetTotalPatientsAsync()
        {
            return await _context.Set<Patient>().CountAsync();
        }
    }
}
