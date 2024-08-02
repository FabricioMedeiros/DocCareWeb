using AutoMapper;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;

namespace DocCareWeb.Application.Services
{
    public class HealthPlanService : GenericService<HealthPlan, HealthPlanCreateDto, HealthPlanUpdateDto, HealthPlanBaseDto>, IHealthPlanService
    {
        public HealthPlanService(
            IGenericRepository<HealthPlan> repository,
            IValidator<HealthPlanCreateDto> createValidator,
            IValidator<HealthPlanUpdateDto> updateValidator,
            IMapper mapper,
            INotificator notificator)
            : base(repository, createValidator, updateValidator, mapper, notificator)
        {
        }
    }
}
