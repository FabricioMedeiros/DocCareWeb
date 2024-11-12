using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace DocCareWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class HealthPlanController : MainController
    {
        private readonly IHealthPlanService _healthPlanService;

        public HealthPlanController(IHealthPlanService healthPlanService, INotificator notificator)
            : base(notificator)
        {
            _healthPlanService = healthPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var healthPlans = await _healthPlanService.GetAllAsync(filters, pageNumber, pageSize);
            return CustomResponse(healthPlans);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var healthPlan = await _healthPlanService.GetByIdAsync(id);

            if (healthPlan == null) return NotFound();

            return CustomResponse(healthPlan);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HealthPlanCreateDto healthPlanDto)
        {
            var createdHealhPlan = await _healthPlanService.AddAsync(healthPlanDto);
            return CustomResponse(createdHealhPlan);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HealthPlanUpdateDto healthPlanDto)
        {
            if (id != healthPlanDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _healthPlanService.UpdateAsync(healthPlanDto);

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var healthPlan = await _healthPlanService.GetByIdAsync(id);

            if (healthPlan == null) return NotFound();

            await _healthPlanService.DeleteAsync(id);

            return CustomResponse();
        }
    }
}
