using MediatR;
using DocCareWeb.Application.Dtos.Specialty;

namespace DocCareWeb.Application.Features.Specialties.Commands;

public sealed record UpdateSpecialtyCommand(SpecialtyUpdateDto Dto) : IRequest<Unit>;