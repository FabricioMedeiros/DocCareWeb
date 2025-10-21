using AutoMapper;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public override async Task<HealthPlanListDto?> AddAsync(HealthPlanCreateDto createDto,
                                                               Func<IQueryable<HealthPlan>, IQueryable<HealthPlan>>? includes = null)
        {
            var isValid = await ValidateIdsAsync(
                 createDto.Items,
                 i => i.ServiceId,
                 async ids =>
                 {
                     var result = await _uow.Services.GetAllAsync(s => ids.Contains(s.Id));
                     return result.Items.Select(s => s.Id).ToHashSet();
                 },
                 "ServiceId"
             );

            if (!isValid)
                return null;

            var newHealthPlan = _mapper.Map<HealthPlan>(createDto);
            await _repository.AddAsync(newHealthPlan);
            await _uow.CommitAsync();

            var healthPlan = await _repository.GetByIdAsync(newHealthPlan.Id, includes);

            return _mapper.Map<HealthPlanListDto>(healthPlan);
        }

        public override async Task<HealthPlanListDto?> UpdateAsync(HealthPlanUpdateDto updateDto)
        {
            var isValid = await ValidateIdsAsync(
                updateDto.Items,
                i => i.ServiceId,
                async ids =>
                {
                    var result = await _uow.Services.GetAllAsync(s => ids.Contains(s.Id));
                    return result.Items.Select(s => s.Id).ToHashSet();
                },
                "ServiceId"
            );

            if (!isValid)
                return null;

            var entity = await _repository.GetByIdAsync(updateDto.Id, query =>
                query.Include(h => h.Items));

            if (entity == null)
            {
                Notify("Plano de saúde não encontrado.");
                return null;
            }

            _mapper.Map(updateDto, entity);

            entity.Items.Clear();

            foreach (var itemDto in updateDto.Items)
            {
                entity.Items.Add(new HealthPlanItem
                {
                    ServiceId = itemDto.ServiceId,
                    Price = itemDto.Price
                });
            }

            _repository.Update(entity);
            await _uow.CommitAsync();

            var updatedEntity = await _repository.GetByIdAsync(entity.Id, query =>
                query.Include(h => h.Items)
                     .ThenInclude(i => i.Service));

            return _mapper.Map<HealthPlanListDto>(updatedEntity);
        }
    }
}
