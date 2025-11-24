using MediatR;

namespace DocCareWeb.Application.Features.Appointments.Commands;

public sealed record DeleteAppointmentCommand(int Id) : IRequest<Unit>;
