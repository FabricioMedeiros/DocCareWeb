using AutoMapper;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Features.Appointments.Commands;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Appointments.Handlers
{
    public sealed class AppointmentCommandHandler :
        IRequestHandler<CreateAppointmentCommand, AppointmentListDto?>,
        IRequestHandler<UpdateAppointmentCommand, Unit>,
        IRequestHandler<UpdateStatusAppointmentCommand, Unit>,
        IRequestHandler<DeleteAppointmentCommand, Unit>
    {
        private readonly IAppointmentService _service;
        private readonly INotificator _notificator;
        private readonly IMapper _mapper;

        public AppointmentCommandHandler(IAppointmentService service, INotificator notificator, IMapper mapper)
        {
            _service = service;
            _notificator = notificator;
            _mapper = mapper;
        }

        public async Task<AppointmentListDto?> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = _mapper.Map<Appointment>(request.Dto);

            return await _service.AddAsync(appointment);
        }

        public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var Appointment = await _service.GetByIdAsync(request.Dto.Id, true);

            if (Appointment == null)
            {
                _notificator.AddNotification(new Notification("Agendamento não encontrado."));
                return Unit.Value;
            }

            _mapper.Map(request.Dto, Appointment);

            await _service.UpdateAsync(Appointment);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateStatusAppointmentCommand request, CancellationToken cancellationToken)
        {
            var Appointment = await _service.GetByIdAsync(request.Dto.Id, true);

            if (Appointment == null)
            {
                _notificator.AddNotification(new Notification("Agendamento não encontrado."));
                return Unit.Value;
            }

            await _service.ChangeStatusAsync(Appointment, request.Dto.Status);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var Appointment = await _service.GetByIdAsync(request.Id, returnEntity: true);

            if (Appointment == null)
            {
                _notificator.AddNotification(new Notification("Agendamento não encontrado."));
                return Unit.Value;
            }

            await _service.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
