using DocCareWeb.Application.Dtos.Service;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace DocCareWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ServiceController : MainController
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService, INotificator notificator)
            : base(notificator)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters,
                                                [FromQuery] int? pageNumber = null,
                                                [FromQuery] int? pageSize = null)
        {
            var services = await _serviceService.GetAllAsync(filters, pageNumber, pageSize);
            return CustomResponse(services);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);

            if (service == null) return NotFound();

            return CustomResponse(service);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCreateDto serviceDto)
        {
            var createdService = await _serviceService.AddAsync(serviceDto);
            return CustomResponse(createdService);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceUpdateDto serviceDto)
        {
            if (id != serviceDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _serviceService.UpdateAsync(serviceDto);

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);

            if (service == null) return NotFound();

            await _serviceService.DeleteAsync(id);

            return CustomResponse();
        }
    }
}
