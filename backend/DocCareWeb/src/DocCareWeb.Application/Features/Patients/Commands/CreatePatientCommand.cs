using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Patients.Commands;

public sealed record CreatePatientCommand(
    PatientCreateDto Dto,
    string CreatedBy,
    DateTime CreatedAt,
    Func<IQueryable<Patient>, IQueryable<Patient>>? Includes
) : IRequest<PatientListDto?>;