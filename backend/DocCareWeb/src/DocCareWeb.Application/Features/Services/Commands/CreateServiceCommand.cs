using MediatR;
using DocCareWeb.Application.Dtos.Service;

namespace DocCareWeb.Application.Features.Services.Commands;

public sealed record CreateServiceCommand(ServiceCreateDto Dto) : IRequest<ServiceListDto?>;