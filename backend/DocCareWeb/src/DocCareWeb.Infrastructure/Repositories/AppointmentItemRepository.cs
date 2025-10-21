using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class AppointmentItemRepository : GenericRepository<AppointmentItem>, IAppointmentItemRepository
    {
        public AppointmentItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
