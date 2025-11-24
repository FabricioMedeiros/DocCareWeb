using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Features.Patients.Commands;
using DocCareWeb.Application.Features.Patients.Queries;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DocCareWeb.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PatientController : MainController
    {
        private readonly IMediator _mediator;
        public PatientController(IMediator mediator,
                                 INotificator notificator) : base(notificator)
        {
          _mediator = mediator; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, 
                                                [FromQuery] int? pageNumber = null, 
                                                [FromQuery] int? pageSize = null)
        {
            var result = await _mediator.Send(new GetAllPatientsQuery(filters,
                                                                      pageNumber,
                                                                      pageSize,
                                                                      IncludePatientRelations()));
            return CustomResponse(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPatientByIdQuery(id,
                                                                      IncludePatientRelations()));

            if (result == null) return NotFound();

            return CustomResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateDto patientDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
            var createdAt = DateTime.Now;

            var result = await _mediator.Send(new CreatePatientCommand(patientDto,
                                                                       userId,
                                                                       createdAt,
                                                                       IncludePatientRelations()));
            return CustomResponse(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDto patientDto)
        {
            if (id != patientDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
            var LastUpdatedAt = DateTime.UtcNow;

            await _mediator.Send(new UpdatePatientCommand(patientDto,
                                                          userId,
                                                          LastUpdatedAt,
                                                          IncludePatientRelations()));

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePatientCommand(id));

            return CustomResponse();
        }

        private static Func<IQueryable<Patient>, IQueryable<Patient>> IncludePatientRelations()
        {
            return query => query
                .Include(p => p.Address)
                .Include(p => p.HealthPlan);
        }
    }
}
