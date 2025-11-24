using AutoMapper;
using MediatR;
using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Features.Specialties.Commands;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;

namespace DocCareWeb.Application.Features.Specialties.Handlers;

public sealed class SpecialtyCommandHandler :
    IRequestHandler<CreateSpecialtyCommand, SpecialtyListDto?>,
    IRequestHandler<UpdateSpecialtyCommand, Unit>,
    IRequestHandler<DeleteSpecialtyCommand, Unit>
{
    private readonly ISpecialtyService _service;
    private readonly INotificator _notificator;
    private readonly IMapper _mapper;

    public SpecialtyCommandHandler(ISpecialtyService service, INotificator notificator, IMapper mapper)
    {
        _service = service;
        _notificator = notificator;
        _mapper = mapper;
    }

    public async Task<SpecialtyListDto?> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var exists = await _service.ExistsAsync(s => s.Name.Trim() == request.Dto.Name.Trim());

        if (exists)
        {
            _notificator.AddNotification(new Notification("Já existe uma especialidade com esse nome."));
            return null!;
        }

        return await _service.AddAsync(request.Dto);
    }

    public async Task<Unit> Handle(UpdateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = await _service.GetByIdAsync(request.Dto.Id, returnEntity: true);

        if (specialty == null)
        {
            _notificator.AddNotification(new Notification("Especialidade não encontrada."));
            return Unit.Value;
        }

        var nameUsed = await _service.ExistsAsync(s => s.Name.Trim() == request.Dto.Name.Trim() && s.Id != request.Dto.Id);

        if (nameUsed)
        {
            _notificator.AddNotification(new Notification("Já existe outra especialidade com esse nome."));
            return Unit.Value;
        }

        _mapper.Map(request.Dto, specialty);
        await _service.UpdateAsync(specialty);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = await _service.GetByIdAsync(request.Id, returnEntity: true);

        if (specialty == null)
        {
            _notificator.AddNotification(new Notification("Especialidade não encontrada."));
            return Unit.Value;
        }

        await _service.DeleteAsync(request.Id);
        return Unit.Value;
    }
}