using AutoMapper;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;

namespace DocCareWeb.Application.Services
{
    public class AppointmentService : GenericService<Appointment, AppointmentCreateDto, AppointmentUpdateDto, AppointmentBaseDto>, IAppointmentService
    {
        private readonly IGenericRepository<Appointment> _appointmentRepository;

        public AppointmentService(
            IGenericRepository<Appointment> appointmentRepository,
            IValidator<AppointmentCreateDto> createValidator,
            IValidator<AppointmentUpdateDto> updateValidator,
            IMapper mapper,
            INotificator notificator)
            : base(appointmentRepository, createValidator, updateValidator, mapper, notificator)
        {
            _appointmentRepository = appointmentRepository;
        }

        public override async Task AddAsync(AppointmentCreateDto createDto)
        {
            var existingAppointments = await _appointmentRepository.GetAllAsync(a =>
                a.DoctorId == createDto.DoctorId && a.AppointmentDate == createDto.AppointmentDate && a.AppointmentTime == createDto.AppointmentTime);

            if (existingAppointments.Any())
            {
                Notify("Já existe uma consulta agendada para este médico no mesmo dia e horário.");
                return;
            }

            if (!await ValidateCreateDto(createDto))
            {
                return;
            }

            var entity = _mapper.Map<Appointment>(createDto);
            await _appointmentRepository.AddAsync(entity);
        }

        public override async Task UpdateAsync(AppointmentUpdateDto updateDto)
        {
            var existingAppointments = await _appointmentRepository.GetAllAsync(a =>
                a.DoctorId == updateDto.DoctorId && a.AppointmentDate == updateDto.AppointmentDate && a.AppointmentTime == updateDto.AppointmentTime && a.Id != updateDto.Id);

            if (existingAppointments.Any())
            {
                Notify("Já existe uma consulta agendada para este médico no mesmo dia e horário.");
                return;
            }

            if (!await ValidateUpdateDto(updateDto))
            {
                return;
            }

            var entity = _mapper.Map<Appointment>(updateDto);
            await _appointmentRepository.UpdateAsync(entity);
        }
    }
}
