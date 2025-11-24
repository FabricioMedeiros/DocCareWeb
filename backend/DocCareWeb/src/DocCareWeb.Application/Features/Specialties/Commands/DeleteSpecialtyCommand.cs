using MediatR;

namespace DocCareWeb.Application.Features.Specialties.Commands;

public sealed record DeleteSpecialtyCommand(int Id) : IRequest<Unit>;