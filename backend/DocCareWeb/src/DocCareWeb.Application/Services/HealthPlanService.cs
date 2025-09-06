using AutoMapper;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;

namespace DocCareWeb.Application.Services
{
    public class HealthPlanService : GenericService<HealthPlan, HealthPlanCreateDto, HealthPlanUpdateDto, HealthPlanListDto>, IHealthPlanService
    {
        public HealthPlanService(
            IUnitOfWork uow,
            IMapper mapper,
            INotificator notificator)
            : base(uow, uow.HealthPlans, mapper, notificator)
        {
        }
    }
}
