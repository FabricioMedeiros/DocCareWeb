using MediatR;
using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.Services.Queries;

public sealed record GetAllServicesQuery(
    Dictionary<string, string>? Filters,
    int? PageNumber,
    int? PageSize
) : IRequest<PagedResult<ServiceListDto>>;