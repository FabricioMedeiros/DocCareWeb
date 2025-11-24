using AutoMapper;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Features.Patients.Commands;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Patients.Handlers;

public sealed class PatientCommandHandler :
    IRequestHandler<CreatePatientCommand, PatientListDto?>,
    IRequestHandler<UpdatePatientCommand, Unit>,
    IRequestHandler<DeletePatientCommand, Unit>
{
    private readonly IPatientService _service;
    private readonly INotificator _notificator;
    private readonly IMapper _mapper;

    public PatientCommandHandler(IPatientService service, INotificator notificator, IMapper mapper)
    {
        _service = service;
        _notificator = notificator;
        _mapper = mapper;
    }

    public async Task<PatientListDto?> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var exists = await _service.ExistsAsync(p => p.Cpf.Trim() == request.Dto.Cpf.Trim());

        if (exists)
        {
            _notificator.AddNotification(new Notification("Já existe um paciente com esse documento."));
            return null!;
        }

        var patient = _mapper.Map<Patient>(request.Dto);
        patient.CreatedBy = request.CreatedBy;
        patient.CreatedAt = request.CreatedAt;

        return await _service.AddAsync(patient, request.Includes);
    }

    public async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _service.GetByIdAsync(request.Dto.Id, returnEntity: true);

        if (patient == null)
        {
            _notificator.AddNotification(new Notification("Paciente não encontrado."));
            return Unit.Value;
        }

        var documentUsed = await _service.ExistsAsync(p => p.Cpf.Trim() == request.Dto.Cpf.Trim() && p.Id != request.Dto.Id);

        if (documentUsed)
        {
            _notificator.AddNotification(new Notification("Já existe outro paciente com esse documento."));
            return Unit.Value;
        }

        _mapper.Map(request.Dto, patient);
        patient.LastUpdatedBy = request.LastUpdatedBy;
        patient.LastUpdatedAt = request.LastUpdatedAt;

        await _service.UpdateAsync(patient);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _service.GetByIdAsync(request.Id, returnEntity: true);

        if (patient == null)
        {
            _notificator.AddNotification(new Notification("Paciente não encontrado."));
            return Unit.Value;
        }

        await _service.DeleteAsync(request.Id);
        return Unit.Value;
    }
}