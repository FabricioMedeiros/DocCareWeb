using MediatR;

namespace DocCareWeb.Application.Features.Patients.Commands;

public sealed record DeletePatientCommand(int Id) : IRequest<Unit>;