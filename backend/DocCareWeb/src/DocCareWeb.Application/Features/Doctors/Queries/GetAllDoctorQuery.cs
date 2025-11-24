using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Doctors.Queries
{
    public sealed record GetAllDoctorsQuery(
        Dictionary<string, string>? Filters,
        int? PageNumber,
        int? PageSize,
        Func<IQueryable<Doctor>, IQueryable<Doctor>>? Includes
        ) : IRequest<PagedResult<DoctorListDto>>;
}
