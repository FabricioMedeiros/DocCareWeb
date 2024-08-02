using AutoMapper;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DocCareWeb.Application.Services
{
    public class GenericService<TEntity, TCreateDto, TUpdateDto, TDto> : BaseService, IGenericService<TEntity, TCreateDto, TUpdateDto, TDto>
        where TEntity : BaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        private readonly IValidator<TCreateDto> _createValidator;
        private readonly IValidator<TUpdateDto> _updateValidator;
        protected readonly IMapper _mapper;

        public GenericService(
            IGenericRepository<TEntity> repository,
            IValidator<TCreateDto> createValidator,
            IValidator<TUpdateDto> updateValidator,
            IMapper mapper,
            INotificator notificator)
            : base(notificator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public virtual async Task<bool> ValidateCreateDto(TCreateDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Notify(error.ErrorMessage);
                }
                return false;
            }

            return true;
        }

        public virtual async Task<bool> ValidateUpdateDto(TUpdateDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Notify(error.ErrorMessage);
                }
                return false;
            }

            return true;
        }

        public virtual async Task<PagedResult<TDto>> GetAllAsync(Expression<Func<TDto, bool>>? filter = null, int? pageNumber = null, int? pageSize = null)
        {
            Expression<Func<TEntity, bool>>? entityFilter = null;

            if (filter != null)
            {
                entityFilter = _mapper.Map<Expression<Func<TEntity, bool>>>(filter);
            }

            var entities = await _repository.GetAllAsync(entityFilter);
            var entitiesQuery = entities.AsQueryable();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var totalCount = await entitiesQuery.CountAsync();
                var pagedEntities = await entitiesQuery.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync();

                var pagedResult = new PagedResult<TDto>
                {
                    TotalRecords = totalCount,
                    Page = pageNumber.Value,
                    PageSize = pageSize.Value,
                    Items = _mapper.Map<IEnumerable<TDto>>(pagedEntities)
                };

                return pagedResult;
            }
            else
            {
                var result = new PagedResult<TDto>
                {
                    TotalRecords = entitiesQuery.Count(),
                    Page = 1,
                    PageSize = entitiesQuery.Count(),
                    Items = _mapper.Map<IEnumerable<TDto>>(entitiesQuery)
                };

                return result;
            }
        }

        public async Task<TDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task AddAsync(TCreateDto createDto)
        {
            if (!await ValidateCreateDto(createDto))
                return;

            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(TUpdateDto updateDto)
        {
            if (!await ValidateUpdateDto(updateDto))
                return;

            var entity = _mapper.Map<TEntity>(updateDto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
