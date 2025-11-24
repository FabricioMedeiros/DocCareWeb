using AutoMapper;
using MediatR;
using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Application.Features.Services.Commands;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;

namespace DocCareWeb.Application.Features.Services.Handlers;

public sealed class ServiceCommandHandler :
    IRequestHandler<CreateServiceCommand, ServiceListDto?>,
    IRequestHandler<UpdateServiceCommand, Unit>,
    IRequestHandler<DeleteServiceCommand, Unit>
{
    private readonly IServiceService _service;
    private readonly INotificator _notificator;
    private readonly IMapper _mapper;

    public ServiceCommandHandler(IServiceService service, INotificator notificator, IMapper mapper)
    {
        _service = service;
        _notificator = notificator;
        _mapper = mapper;
    }

    public async Task<ServiceListDto?> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var exists = await _service.ExistsAsync(s => s.Name.Trim() == request.Dto.Name.Trim());

        if (exists)
        {
            _notificator.AddNotification(new Notification("Já existe um serviço com esse nome."));
            return null!;
        }

        return await _service.AddAsync(request.Dto);
    }

    public async Task<Unit> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _service.GetByIdAsync(request.Dto.Id, returnEntity: true);

        if (entity == null)
        {
            _notificator.AddNotification(new Notification("Serviço não encontrado."));
            return Unit.Value;
        }

        var nameUsed = await _service.ExistsAsync(s => s.Name.Trim() == request.Dto.Name.Trim() && s.Id != request.Dto.Id);

        if (nameUsed)
        {
            _notificator.AddNotification(new Notification("Já existe outro serviço com esse nome."));
            return Unit.Value;
        }

        _mapper.Map(request.Dto, entity);
        await _service.UpdateAsync(entity);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _service.GetByIdAsync(request.Id, returnEntity: true);

        if (entity == null)
        {
            _notificator.AddNotification(new Notification("Serviço não encontrado."));
            return Unit.Value;
        }

        await _service.DeleteAsync(request.Id);
        return Unit.Value;
    }
}