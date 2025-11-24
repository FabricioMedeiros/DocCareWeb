using MediatR;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.Specialties.Queries;

public sealed record GetSpecialtyByIdQuery(
    int Id
) : IRequest<SpecialtyListDto?>;