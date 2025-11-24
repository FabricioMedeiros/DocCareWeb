using MediatR;

namespace DocCareWeb.Application.Features.Services.Commands;

public sealed record DeleteServiceCommand(int Id) : IRequest<Unit>;