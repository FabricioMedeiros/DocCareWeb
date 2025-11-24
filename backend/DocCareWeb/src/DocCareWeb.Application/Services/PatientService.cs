using AutoMapper;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Services;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;

public class PatientService : GenericService<Patient, PatientCreateDto, PatientUpdateDto, PatientListDto>, IPatientService
{
    public PatientService(
        IUnitOfWork uow,
        IPatientRepository repository,
        IMapper mapper,
        INotificator notificator)
        : base(uow, uow.Patients, mapper, notificator)
    {
    }

    public override async Task<PatientListDto?> AddAsync(Patient entity,
                                                         Func<IQueryable<Patient>, IQueryable<Patient>>? includes = null)
    {
        var healthPlan = await _uow.HealthPlans.GetByIdAsync(entity.HealthPlanId);
        
        if (healthPlan  == null)
        {
            Notify("O plano de saúde especificado não existe.");
            return null;
        }

        await _repository.AddAsync(entity);
        await _uow.CommitAsync();

        var patient = await _repository.GetByIdAsync(entity.Id, includes);
        return _mapper.Map<PatientListDto>(patient);
    }

    public override async Task UpdateAsync(Patient entity)
    {
        var healthPlan = await _uow.HealthPlans.GetByIdAsync(entity.HealthPlanId);

        if (healthPlan == null)
        {
            Notify("O plano de saúde especificado não existe.");
            return;
        }

        _repository.Update(entity);
        await _uow.CommitAsync();
    }
}
