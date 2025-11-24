using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Appointments.Queries
{
    public sealed record GetAppointmentByIdQuery(
        int Id,
        Func<IQueryable<Appointment>, IQueryable<Appointment>>? Includes)
        : IRequest<AppointmentListDto?>;
}
