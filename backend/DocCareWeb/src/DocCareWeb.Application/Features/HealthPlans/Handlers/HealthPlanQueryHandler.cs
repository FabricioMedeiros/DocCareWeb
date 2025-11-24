using MediatR;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Features.HealthPlans.Queries;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.HealthPlans.Handlers;

public sealed class HealthPlanQueryHandler :
    IRequestHandler<GetAllHealthPlansQuery, PagedResult<HealthPlanListDto>>,
    IRequestHandler<GetHealthPlanByIdQuery, HealthPlanListDto?>
{
    private readonly IHealthPlanService _service;

    public HealthPlanQueryHandler(IHealthPlanService service)
    {
        _service = service;
    }

    public async Task<PagedResult<HealthPlanListDto>> Handle(GetAllHealthPlansQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(
            request.Filters,
            request.PageNumber,
            request.PageSize,
            includes: request.Includes ?? (q => q)
        );
    }

    public async Task<HealthPlanListDto?> Handle(GetHealthPlanByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(
            request.Id,
            includes: request.Includes ?? (q => q)
        );
    }
}