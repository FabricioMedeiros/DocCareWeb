using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Features.HealthPlans.Commands;
using DocCareWeb.Application.Features.HealthPlans.Queries;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocCareWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class HealthPlanController : MainController
    {
        private readonly IMediator _mediator;

        public HealthPlanController(
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
            var result = await _mediator.Send(new GetAllHealthPlansQuery(filters,
                                                                         pageNumber,
                                                                         pageSize,
                                                                         IncludeHealthPlanRelations()));
            return CustomResponse(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetHealthPlanByIdQuery(id,
                                                                         IncludeHealthPlanRelations()));

            if (result == null) return NotFound();

            return CustomResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HealthPlanCreateDto healthPlanDto)
        {
            var result = await _mediator.Send(new CreateHealthPlanCommand(healthPlanDto,
                                                                          IncludeHealthPlanRelations()));
            return CustomResponse(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HealthPlanUpdateDto healthPlanDto)
        {
            if (id != healthPlanDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdateHealthPlanCommand(healthPlanDto));

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteHealthPlanCommand(id));

            return CustomResponse();
        }
        private static Func<IQueryable<HealthPlan>, IQueryable<HealthPlan>> IncludeHealthPlanRelations()
        {
            return query => query
                .Include(h => h.Items)
                .ThenInclude(i => i.Service);
        }
    }
}
