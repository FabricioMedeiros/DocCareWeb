using MediatR;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.Patients.Queries;

public sealed record GetAllPatientsQuery(
    Dictionary<string, string>? Filters,
    int? PageNumber,
    int? PageSize,
    Func<IQueryable<Patient>, IQueryable<Patient>>? Includes
) : IRequest<PagedResult<PatientListDto>>;