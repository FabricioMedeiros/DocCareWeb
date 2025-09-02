using AutoMapper;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace DocCareWeb.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DoctorController : MainController
    {
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
    
        public DoctorController(IDoctorService doctorService,
                                IMapper mapper,
                                INotificator notificator) : base(notificator)
        {
            _doctorService = doctorService;
            _mapper = mapper;
      
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var doctors = await _doctorService.GetAllAsync(filters,
                                                           pageNumber,
                                                           pageSize,
                                                           includes: new Expression<Func<Doctor, object>>[]
                                                                     {
                                                                        x => x.Specialty!
                                                                     });
            return CustomResponse(doctors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id,
                                                           includes: new Expression<Func<Doctor, object>>[]
                                                                     {
                                                                        x => x.Specialty!
                                                                     });

            if (doctor == null) return NotFound();

            return CustomResponse(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoctorCreateDto doctorDto)
        {
           var doctor = _mapper.Map<Doctor>(doctorDto);

           var createdDoctor = await _doctorService.AddAsync(doctor);
           return CustomResponse(createdDoctor);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorUpdateDto doctorDto)
        {
            if (id != doctorDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }
                        
            var doctor = await _doctorService.GetByIdAsync(id, true);
            if (doctor == null) return NotFound();

            _mapper.Map(doctorDto, doctor);

            await _doctorService.UpdateAsync(doctor);

            return CustomResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor == null) return NotFound();

            await _doctorService.DeleteAsync(id);

            return CustomResponse();
        }
    }
}
