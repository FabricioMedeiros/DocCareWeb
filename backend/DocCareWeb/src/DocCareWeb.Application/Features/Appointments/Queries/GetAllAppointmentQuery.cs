using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Appointments.Queries
{
    public sealed record GetAllAppointmentsQuery(
    Dictionary<string, string>? Filters,
    int? PageNumber,
    int? PageSize,
    Func<IQueryable<Appointment>, IQueryable<Appointment>>? Includes) 
    : IRequest<PagedResult<AppointmentListDto>>;
}
