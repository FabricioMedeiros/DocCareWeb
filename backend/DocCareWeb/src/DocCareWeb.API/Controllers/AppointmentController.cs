using AutoMapper;
using DocCareWeb.API.Filters;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocCareWeb.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AppointmentController : MainController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        public AppointmentController(IAppointmentService appointmentService,
                                      IMapper mapper,
                                      INotificator notificator) : base(notificator)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([ModelBinder(BinderType = typeof(FilterBinder))] Dictionary<string, string>? filters,
                                                [FromQuery] int? pageNumber = null,
                                                [FromQuery] int? pageSize = null)
        {
            var appointments = await _appointmentService.GetAllAsync(filters, pageNumber, pageSize);
            return CustomResponse(appointments);
        }      

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment == null) return NotFound();

            return CustomResponse(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto appointmentDto)
        {       
            var appointment = _mapper.Map<Appointment>(appointmentDto);
            
            var createdAppointment = await _appointmentService.AddAsync(appointment);
            return CustomResponse(createdAppointment);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentUpdateDto appointmentDto)
        {
            if (id != appointmentDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }


            var appointment = await _appointmentService.GetByIdAsync(id, true);

            if (appointment == null) return NotFound();

            _mapper.Map(appointmentDto, appointment);           

            var result = await _appointmentService.UpdateAsync(appointment);

            if (!result) CustomResponse();

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
            var appointment = await _appointmentService.GetByIdAsync(statusDto.Id, true);

            if (appointment == null)
                return NotFound();                      

            var result = await _appointmentService.ChangeStatusAsync(appointment, statusDto.Status);

            if (!result) return CustomResponse();

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment == null) return NotFound();

            await _appointmentService.DeleteAsync(id);

            return CustomResponse();
        }
    }
}
