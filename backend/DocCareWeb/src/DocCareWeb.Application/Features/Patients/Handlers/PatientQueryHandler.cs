using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Features.Patients.Queries;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Patients.Handlers;

public sealed class PatientQueryHandler :
    IRequestHandler<GetAllPatientsQuery, PagedResult<PatientListDto>>,
    IRequestHandler<GetPatientByIdQuery, PatientListDto?>
{
    private readonly IPatientService _service;

    public PatientQueryHandler(IPatientService service)
    {
        _service = service;
    }

    public async Task<PagedResult<PatientListDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(
            request.Filters,
            request.PageNumber,
            request.PageSize,
            includes: request.Includes
        );
    }

    public async Task<PatientListDto?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(
            request.Id,
            includes: request.Includes
        );
    }
}