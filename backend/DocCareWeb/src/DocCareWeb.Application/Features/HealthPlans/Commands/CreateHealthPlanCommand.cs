using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.HealthPlans.Commands;

public sealed record CreateHealthPlanCommand(
    HealthPlanCreateDto Dto,
    Func<IQueryable<HealthPlan>, IQueryable<HealthPlan>>? Includes
    ) : IRequest<HealthPlanListDto?>;