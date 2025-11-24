using MediatR;
using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Features.Services.Queries;

public sealed record GetServiceByIdQuery(int Id) : IRequest<ServiceListDto?>;