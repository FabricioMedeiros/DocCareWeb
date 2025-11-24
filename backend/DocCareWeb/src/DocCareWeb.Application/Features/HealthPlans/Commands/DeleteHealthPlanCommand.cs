using MediatR;

namespace DocCareWeb.Application.Features.HealthPlans.Commands;

public sealed record DeleteHealthPlanCommand(int Id) : IRequest<Unit>;