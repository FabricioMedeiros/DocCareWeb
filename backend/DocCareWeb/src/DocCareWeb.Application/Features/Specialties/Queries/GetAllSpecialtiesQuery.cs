using MediatR;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.Specialties.Queries;

public sealed record GetAllSpecialtiesQuery(
    Dictionary<string, string>? Filters,
    int? PageNumber,
    int? PageSize
) : IRequest<PagedResult<SpecialtyListDto>>;