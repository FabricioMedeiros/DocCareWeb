using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Features.Doctors.Commands;
using DocCareWeb.Application.Features.Doctors.Queries;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocCareWeb.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DoctorController : MainController
    {
        private readonly IMediator _mediator;

        public DoctorController(
            IMediator mediator,
            INotificator notificator) : base(notificator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters,
                                                [FromQuery] int? pageNumber = null,
                                                [FromQuery] int? pageSize = null)
        {
            var result = await _mediator.Send(new GetAllDoctorsQuery(filters,
                                                                     pageNumber,
                                                                     pageSize,
                                                                     IncludeDoctorRelations()));
            return CustomResponse(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDoctorByIdQuery(id,
                                                                     IncludeDoctorRelations()));

            if (result == null) return NotFound();

            return CustomResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoctorCreateDto doctorDto)
        {
            var result = await _mediator.Send(new CreateDoctorCommand(doctorDto,
                                                                      IncludeDoctorRelations()));
            return CustomResponse(result);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorUpdateDto doctorDto)
        {
            if (id != doctorDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdateDoctorCommand(doctorDto));

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteDoctorCommand(id));

            return CustomResponse();
        }

        private static Func<IQueryable<Doctor>, IQueryable<Doctor>> IncludeDoctorRelations() 
        { 
            return query => query.Include(d => d.Specialty); 
        }
    }
}
