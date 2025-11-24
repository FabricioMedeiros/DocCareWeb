using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Features.Appointments.Commands;
using DocCareWeb.Application.Features.Appointments.Queries;
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
    public class AppointmentController : MainController
    {
        private readonly IMediator _mediator;
        public AppointmentController(
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
            var result = await _mediator.Send(new GetAllAppointmentsQuery(filters,
                                                                          pageNumber,
                                                                          pageSize,
                                                                          IncludeAppointmentRelations()));
            return CustomResponse(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAppointmentByIdQuery(id,
                                                                          IncludeAppointmentRelations()));
            if (result == null) return NotFound();

            return CustomResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto appointmentDto)
        {
            var result = await _mediator.Send(new CreateAppointmentCommand(appointmentDto,
                                                                          IncludeAppointmentRelations()));
            return CustomResponse(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentUpdateDto appointmentDto)
        {
            if (id != appointmentDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdateAppointmentCommand(appointmentDto,
                                                              IncludeAppointmentRelations()));

            return CustomResponse();
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] AppointmentStatusDto statusDto)
        {
            if (id != statusDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdateStatusAppointmentCommand(statusDto,
                                                                    IncludeAppointmentRelations()));

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAppointmentCommand(id));

            return CustomResponse();
        }

        private static Func<IQueryable<Appointment>, IQueryable<Appointment>> IncludeAppointmentRelations()
        {
            return query => query
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.HealthPlan)
                .Include(a => a.Items)
                .ThenInclude(i => i.Service);
        }
    }
}
