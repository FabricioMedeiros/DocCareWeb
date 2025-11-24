using MediatR;
using DocCareWeb.Application.Dtos.HealthPlan;

namespace DocCareWeb.Application.Features.HealthPlans.Commands;

public sealed record UpdateHealthPlanCommand(HealthPlanUpdateDto Dto) : IRequest<Unit>;