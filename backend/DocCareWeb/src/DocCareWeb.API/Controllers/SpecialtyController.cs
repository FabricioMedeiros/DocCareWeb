using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Features.Specialties.Commands;
using DocCareWeb.Application.Features.Specialties.Queries;
using DocCareWeb.Application.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocCareWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class SpecialtyController : MainController
    {
        private readonly IMediator _mediator;

        public SpecialtyController(
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
            var result = await _mediator.Send(new GetAllSpecialtiesQuery(filters, pageNumber, pageSize));
            return CustomResponse(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSpecialtyByIdQuery(id));
            if (result == null) return NotFound();
            return CustomResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecialtyCreateDto specialtyDto)
        {
            var created = await _mediator.Send(new CreateSpecialtyCommand(specialtyDto));
            return CustomResponse(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SpecialtyUpdateDto specialtyDto)
        {
            if (id != specialtyDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdateSpecialtyCommand(specialtyDto));
            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteSpecialtyCommand(id));
            return CustomResponse();
        }
    }
}
