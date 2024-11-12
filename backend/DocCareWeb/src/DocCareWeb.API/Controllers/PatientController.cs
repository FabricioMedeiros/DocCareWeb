using AutoMapper;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DocCareWeb.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PatientController : MainController
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        public PatientController(IPatientService patientService, 
                                 IMapper mapper, 
                                 INotificator notificator) : base(notificator)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var patients = await _patientService.GetAllAsync(filters, pageNumber, pageSize);
            return CustomResponse(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null) return NotFound();

            return CustomResponse(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateDto patientDto)
        {         
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = _mapper.Map<Patient>(patientDto);
            patient.CreatedBy = userId!;
            patient.CreatedAt = DateTime.Now;

            var createdPatient = await _patientService.AddAsync(patient);
            return CustomResponse(createdPatient);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDto patientDto)
        {
            if (id != patientDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = await _patientService.GetByIdAsync(id, true);
            if (patient == null) return NotFound();

            _mapper.Map(patientDto, patient);
            patient.LastUpdatedBy = userId;
            patient.LastUpdatedAt = DateTime.UtcNow;

            await _patientService.UpdateAsync(patient);

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null) return NotFound();

            await _patientService.DeleteAsync(id);

            return CustomResponse();
        }
    }
}
