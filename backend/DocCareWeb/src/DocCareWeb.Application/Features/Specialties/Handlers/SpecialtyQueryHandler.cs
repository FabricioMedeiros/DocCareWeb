using MediatR;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Features.Specialties.Queries;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.Specialties.Handlers;

public sealed class SpecialtyQueryHandler :
    IRequestHandler<GetAllSpecialtiesQuery, PagedResult<SpecialtyListDto>>,
    IRequestHandler<GetSpecialtyByIdQuery, SpecialtyListDto?>
{
    private readonly ISpecialtyService _service;

    public SpecialtyQueryHandler(ISpecialtyService service)
    {
        _service = service;
    }

    public async Task<PagedResult<SpecialtyListDto>> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(
            request.Filters,
            request.PageNumber,
            request.PageSize
        );
    }

    public async Task<SpecialtyListDto?> Handle(GetSpecialtyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(request.Id);
    }
}