using MediatR;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.HealthPlans.Queries;

public sealed record GetHealthPlanByIdQuery(
    int Id,
    Func<IQueryable<HealthPlan>, IQueryable<HealthPlan>>? Includes
) : IRequest<HealthPlanListDto?>;