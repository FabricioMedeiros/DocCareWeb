using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Patients.Commands;

public sealed record UpdatePatientCommand(
    PatientUpdateDto Dto,
    string LastUpdatedBy,
    DateTime LastUpdatedAt,
    Func<IQueryable<Patient>, IQueryable<Patient>>? Includes) : IRequest<Unit>;