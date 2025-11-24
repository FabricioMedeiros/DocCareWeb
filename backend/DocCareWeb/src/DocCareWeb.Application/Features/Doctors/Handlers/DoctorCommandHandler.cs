using AutoMapper;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Features.Doctors.Commands;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using MediatR;

namespace DocCareWeb.Application.Features.Doctors.Handlers
{
    public sealed class DoctorCommandHandler :
        IRequestHandler<CreateDoctorCommand, DoctorListDto?>,
        IRequestHandler<UpdateDoctorCommand, Unit>,
        IRequestHandler<DeleteDoctorCommand, Unit>
    {
        private readonly IDoctorService _service;
        private readonly INotificator _notificator;
        private readonly IMapper _mapper;

        public DoctorCommandHandler(IDoctorService service, INotificator notificator, IMapper mapper)
        {
            _service = service;
            _notificator = notificator;
            _mapper = mapper;
        }

        public async Task<DoctorListDto?> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            return await _service.AddAsync(request.Dto);
        }

        public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var Doctor = await _service.GetByIdAsync(request.Dto.Id, true);

            if (Doctor == null)
            {
                _notificator.AddNotification(new Notification("Médico não encontrado."));
                return Unit.Value;
            }                      

            _mapper.Map(request.Dto, Doctor);

            await _service.UpdateAsync(Doctor);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var Doctor = await _service.GetByIdAsync(request.Id, returnEntity: true);

            if (Doctor == null)
            {
                _notificator.AddNotification(new Notification("Médico não encontrado."));
                return Unit.Value;
            }

            await _service.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
