using AutoMapper;
using MediatR;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Features.HealthPlans.Commands;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;

namespace DocCareWeb.Application.Features.HealthPlans.Handlers;

public sealed class HealthPlanCommandHandler :
    IRequestHandler<CreateHealthPlanCommand, HealthPlanListDto?>,
    IRequestHandler<UpdateHealthPlanCommand, Unit>,
    IRequestHandler<DeleteHealthPlanCommand, Unit>
{
    private readonly IHealthPlanService _service;
    private readonly INotificator _notificator;
    private readonly IMapper _mapper;

    public HealthPlanCommandHandler(IHealthPlanService service, INotificator notificator, IMapper mapper)
    {
        _service = service;
        _notificator = notificator;
        _mapper = mapper;
    }

    public async Task<HealthPlanListDto?> Handle(CreateHealthPlanCommand request, CancellationToken cancellationToken)
    {
        var exists = await _service.ExistsAsync(p => p.Name.Trim() == request.Dto.Name.Trim());

        if (exists)
        {
            _notificator.AddNotification(new Notification("Já existe um plano de saúde com esse nome."));
            return null!;
        }

        return await _service.AddAsync(request.Dto);
    }

    public async Task<Unit> Handle(UpdateHealthPlanCommand request, CancellationToken cancellationToken)
    {
        var entity = await _service.GetByIdAsync(request.Dto.Id, returnEntity: true);

        if (entity == null)
        {
            _notificator.AddNotification(new Notification("Plano de Saúde não encontrado."));
            return Unit.Value;
        }

        var nameUsed = await _service.ExistsAsync(p => p.Name.Trim() == request.Dto.Name.Trim() && p.Id != request.Dto.Id);

        if (nameUsed)
        {
            _notificator.AddNotification(new Notification("Já existe outro plano de saúde com esse nome."));
            return Unit.Value;
        }

        _mapper.Map(request.Dto, entity);
        await _service.UpdateAsync(entity);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteHealthPlanCommand request, CancellationToken cancellationToken)
    {
        var entity = await _service.GetByIdAsync(request.Id, returnEntity: true);

        if (entity == null)
        {
            _notificator.AddNotification(new Notification("Convênio não encontrado."));
            return Unit.Value;
        }

        await _service.DeleteAsync(request.Id);
        return Unit.Value;
    }
}