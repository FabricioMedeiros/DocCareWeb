using DocCareWeb.Application.Dtos.Specialty;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace DocCareWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class SpecialtyController : MainController
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtyController(ISpecialtyService specialtyService, INotificator notificator)
            : base(notificator)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var specialties = await _specialtyService.GetAllAsync(filters, pageNumber, pageSize);
            return CustomResponse(specialties);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var specialty = await _specialtyService.GetByIdAsync(id);

            if (specialty == null) return NotFound();

            return CustomResponse(specialty);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecialtyCreateDto specialtyDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var createdSpecialty = await _specialtyService.AddAsync(specialtyDto);
            return CustomResponse(createdSpecialty);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SpecialtyUpdateDto specialtyDto)
        {
            if (id != specialtyDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _specialtyService.UpdateAsync(specialtyDto);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var specialty = await _specialtyService.GetByIdAsync(id);

            if (specialty == null) return NotFound();

            await _specialtyService.DeleteAsync(id);

            return CustomResponse();
        }
    }
}
