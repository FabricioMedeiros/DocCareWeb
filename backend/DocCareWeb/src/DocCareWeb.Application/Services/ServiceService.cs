using AutoMapper;
using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocCareWeb.Application.Services
{
    public class ServiceService : GenericService<Service, ServiceCreateDto, ServiceUpdateDto, ServiceListDto>, IServiceService
    {
        public ServiceService(IUnitOfWork uow,
                              IGenericRepository<Service> repository,
                              IMapper mapper,
                              INotificator notificator) : base(uow, repository, mapper, notificator)
        {
        }

        public override async Task<PagedResult<ServiceListDto>> GetAllAsync(
            Dictionary<string, string>? filters,
            int? pageNumber = null,
            int? pageSize = null,
            Func<IQueryable<Service>, IQueryable<Service>>? includes = null)
        {
            int? healthPlanId = null;

            if (filters != null)
            {
                var match = filters.FirstOrDefault(f => f.Key.Equals("healthPlanId", StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(match.Value) && int.TryParse(match.Value, out var parsedId))
                {
                    healthPlanId = parsedId;
                    filters.Remove(match.Key);
                }
            }

            var filterExpression = ApplyFilters(filters);

            int? skip = null;
            int? take = null;

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                skip = (pageNumber.Value - 1) * pageSize.Value;
                take = pageSize.Value;
            }

            Func<IQueryable<Service>, IQueryable<Service>> composedIncludes = query =>
            {
                var baseQuery = includes != null ? includes(query) : query;
                if (healthPlanId.HasValue)
                {
                    baseQuery = baseQuery.Include(s => s.HealthPlanItems);
                }
                return baseQuery.AsNoTracking();
            };

            var (services, totalRecords) = await _repository.GetAllAsync(
                filter: filterExpression,
                includes: composedIncludes,
                skip: skip,
                take: take);

            var dtos = _mapper.Map<List<ServiceListDto>>(services);

            if (healthPlanId.HasValue)
            {
                foreach (var dto in dtos)
                {
                    var service = services.FirstOrDefault(s => s.Id == dto.Id);
                    var planItem = service?.HealthPlanItems.FirstOrDefault(p => p.HealthPlanId == healthPlanId);

                    if (planItem != null)
                    {
                        dto.Price = planItem.Price;
                        dto.IsHealthPlanPrice = true;
                    }
                }
            }

            return new PagedResult<ServiceListDto>
            {
                Page = pageNumber ?? 1,
                PageSize = pageSize ?? totalRecords,
                TotalRecords = totalRecords,
                Items = dtos
            };
        }
    }
}