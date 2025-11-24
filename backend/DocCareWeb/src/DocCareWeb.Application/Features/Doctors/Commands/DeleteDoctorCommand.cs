using MediatR;

namespace DocCareWeb.Application.Features.Doctors.Commands
{
    public sealed record DeleteDoctorCommand(int Id) : IRequest<Unit>;
}
