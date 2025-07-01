using AutoMapper;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Utils;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;
using System.Linq.Expressions;

namespace DocCareWeb.Application.Services
{
    public class GenericService<TEntity, TCreateDto, TUpdateDto, TListDto> : BaseService, IGenericService<TEntity, TCreateDto, TUpdateDto, TListDto>
        where TEntity : BaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TListDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
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
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<TListDto>> GetAllAsync(
                                  Dictionary<string, string>? filters,
                                  int? pageNumber = null,
                                  int? pageSize = null,
                                  params Expression<Func<TEntity, object>>[] includes)
                                        {
            var filterExpression = ApplyFilters(filters);

            int page = pageNumber ?? 1;
            int size = pageSize ?? 10;
            int skip = (page - 1) * size;

            var pagedItems = await _repository.GetAllAsync(filterExpression, skip, size, includes);

            return new PagedResult<TListDto>
            {
                Page = page,
                PageSize = size,
                TotalRecords = pagedItems.Count(),
                Items = _mapper.Map<IEnumerable<TListDto>>(pagedItems)
            };
        }

        public virtual async Task<TListDto?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = await _repository.GetByIdAsync(id, includes);
            return _mapper.Map<TListDto>(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, bool returnEntity)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<TListDto?> AddAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TListDto>(createdEntity);
        }

        public virtual async Task<TListDto?> AddAsync(TEntity entity)
        {
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TListDto>(createdEntity);
        }

        public virtual async Task UpdateAsync(TUpdateDto updateDto)
        {
            var entity = _mapper.Map<TEntity>(updateDto);
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private static Expression<Func<TEntity, bool>> ApplyFilters(Dictionary<string, string>? filters)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression combinedExpression = Expression.Constant(true);

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    try
                    {
                        var comparison = ExpressionHelper.CreateNestedComparisonExpression(parameter, filter.Key, filter.Value);
                        combinedExpression = Expression.AndAlso(combinedExpression, comparison);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
        }
    }
}
