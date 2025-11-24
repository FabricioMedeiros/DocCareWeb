using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Doctors.Commands;

public sealed record CreateDoctorCommand(
    DoctorCreateDto Dto,
    Func<IQueryable<Doctor>, IQueryable<Doctor>>? Includes
    ) : IRequest<DoctorListDto?>;
