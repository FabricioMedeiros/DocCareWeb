using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Patients.Queries;

public sealed record GetPatientByIdQuery(
    int Id,
    Func<IQueryable<Patient>, IQueryable<Patient>>? Includes
    ) : IRequest<PatientListDto?>;