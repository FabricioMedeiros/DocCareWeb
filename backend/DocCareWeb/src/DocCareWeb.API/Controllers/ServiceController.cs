using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Application.Features.Services.Commands;
using DocCareWeb.Application.Features.Services.Queries;
using DocCareWeb.Application.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocCareWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ServiceController : MainController
    {
        private readonly IMediator _mediator;

        public ServiceController(
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
            var result = await _mediator.Send(new GetAllServicesQuery(filters, pageNumber, pageSize));
            return CustomResponse(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetServiceByIdQuery(id));
            if (result == null) return NotFound();
            return CustomResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCreateDto serviceDto)
        {
            var created = await _mediator.Send(new CreateServiceCommand(serviceDto));
            return CustomResponse(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceUpdateDto serviceDto)
        {
            if (id != serviceDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdateServiceCommand(serviceDto));
            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteServiceCommand(id));
            return CustomResponse();
        }
    }
}
