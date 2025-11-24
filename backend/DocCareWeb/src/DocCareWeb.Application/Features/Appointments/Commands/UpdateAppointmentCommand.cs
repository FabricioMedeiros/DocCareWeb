using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Appointments.Commands;

public sealed record UpdateAppointmentCommand(
    AppointmentUpdateDto Dto,
    Func<IQueryable<Appointment>, IQueryable<Appointment>>? Includes)
    : IRequest<Unit>;
