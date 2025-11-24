using MediatR;
using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Application.Features.Services.Queries;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.Services.Handlers;

public sealed class ServiceQueryHandler :
    IRequestHandler<GetAllServicesQuery, PagedResult<ServiceListDto>>,
    IRequestHandler<GetServiceByIdQuery, ServiceListDto?>
{
    private readonly IServiceService _service;

    public ServiceQueryHandler(IServiceService service)
    {
        _service = service;
    }

    public async Task<PagedResult<ServiceListDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(
            request.Filters,
            request.PageNumber,
            request.PageSize
        );
    }

    public async Task<ServiceListDto?> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(request.Id);
    }
}