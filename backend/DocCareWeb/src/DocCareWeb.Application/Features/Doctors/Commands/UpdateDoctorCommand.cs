using DocCareWeb.Application.Dtos.Doctor;
using MediatR;

namespace DocCareWeb.Application.Features.Doctors.Commands
{
    public sealed record UpdateDoctorCommand(DoctorUpdateDto Dto) : IRequest<Unit>;
}
