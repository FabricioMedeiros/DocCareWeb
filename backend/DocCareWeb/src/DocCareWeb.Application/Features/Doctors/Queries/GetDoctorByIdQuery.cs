using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Doctors.Queries
{
    public sealed record GetDoctorByIdQuery(
        int Id, 
        Func<IQueryable<Doctor>, IQueryable<Doctor>>? Includes
        ) : IRequest<DoctorListDto?>;
}
