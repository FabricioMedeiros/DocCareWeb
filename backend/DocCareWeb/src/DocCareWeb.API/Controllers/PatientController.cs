using AutoMapper;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using FluentValidation;
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
        private readonly IValidator<PatientCreateDto> _createValidator;
        private readonly IValidator<PatientUpdateDto> _updateValidator;
        public PatientController(IPatientService patientService, 
                                 IMapper mapper, 
                                 INotificator notificator, 
                                 IValidator<PatientCreateDto> createValidator,
                                 IValidator<PatientUpdateDto> updateValidator) : base(notificator)
        {
            _patientService = patientService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
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
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!await _patientService.ValidateCreateDto(patientDto))
                return CustomResponse(); 
            
            var userId = User.FindFirstValue("userId");

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

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!await _patientService.ValidateUpdateDto(patientDto))
                return CustomResponse();

            var userId = User.FindFirstValue("userId");

            var patient = await _patientService.GetByIdAsync(id, true);
            if (patient == null) return NotFound();

            _mapper.Map(patientDto, patient);
            patient.LastUpdatedBy = userId;
            patient.LastUpdatedAt = DateTime.UtcNow;

            await _patientService.UpdateAsync(patient);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null) return NotFound();

            await _patientService.DeleteAsync(id);

            return Ok();
        }
    }
}
