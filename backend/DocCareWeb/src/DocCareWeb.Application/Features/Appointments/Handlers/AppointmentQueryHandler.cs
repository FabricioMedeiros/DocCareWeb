using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Features.Appointments.Queries;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Appointments.Handlers
{
    public sealed class AppointmentQueryHandler :
        IRequestHandler<GetAllAppointmentsQuery, PagedResult<AppointmentListDto>>,
        IRequestHandler<GetAppointmentByIdQuery, AppointmentListDto?>
    {
        private readonly IAppointmentService _service;

        public AppointmentQueryHandler(IAppointmentService service)
        {
            _service = service;
        }

        public async Task<PagedResult<AppointmentListDto>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(
                request.Filters,
                request.PageNumber,
                request.PageSize,
                request.Includes);
        }

        public async Task<AppointmentListDto?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.Id, request.Includes);
        }
    }
}
