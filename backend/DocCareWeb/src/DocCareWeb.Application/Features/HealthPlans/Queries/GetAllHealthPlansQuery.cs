using MediatR;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.HealthPlans.Queries;

public sealed record GetAllHealthPlansQuery(
    Dictionary<string, string>? Filters,
    int? PageNumber,
    int? PageSize,
    Func<IQueryable<HealthPlan>, IQueryable<HealthPlan>>? Includes
) : IRequest<PagedResult<HealthPlanListDto>>;