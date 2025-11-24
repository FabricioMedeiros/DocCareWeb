using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Features.Doctors.Queries;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Entities;
using MediatR;

namespace DocCareWeb.Application.Features.Doctors.Handlers
{
    public sealed class DoctorQueryHandler :
        IRequestHandler<GetAllDoctorsQuery, PagedResult<DoctorListDto>>,
        IRequestHandler<GetDoctorByIdQuery, DoctorListDto?>
    {
        private readonly IDoctorService _service;

        public DoctorQueryHandler(IDoctorService service)
        {
            _service = service;
        }

        public async Task<PagedResult<DoctorListDto>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(
                request.Filters,
                request.PageNumber,
                request.PageSize,
                includes: request.Includes);
        }

        public async Task<DoctorListDto?> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.Id, includes: request.Includes);
        }
    }
}
