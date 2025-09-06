using AutoMapper;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Utils;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using System.Linq.Expressions;

namespace DocCareWeb.Application.Services
{
    public class GenericService<TEntity, TCreateDto, TUpdateDto, TListDto> : BaseService, IGenericService<TEntity, TCreateDto, TUpdateDto, TListDto>
        where TEntity : BaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TListDto : class
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(
            IUnitOfWork uow,
            IGenericRepository<TEntity> repository,
            IMapper mapper,
            INotificator notificator)
            : base(notificator)
        {
            _uow = uow;
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<TListDto>> GetAllAsync(
            Dictionary<string, string>? filters,
            int? pageNumber = null,
            int? pageSize = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var filterExpression = ApplyFilters(filters);

            int page = pageNumber ?? 1;
            int size = pageSize ?? 10;
            int skip = (page - 1) * size;

            var (items, totalRecords) = await _repository.GetAllAsync(
                filter: filterExpression,
                includes: includes,
                skip: skip,
                take: size);

            return new PagedResult<TListDto>
            {
                Page = page,
                PageSize = size,
                TotalRecords = totalRecords,
                Items = _mapper.Map<IEnumerable<TListDto>>(items)
            };
        }

        public virtual async Task<TListDto?> GetByIdAsync(
            int id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
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
            await _repository.AddAsync(entity);
            await _uow.CommitAsync();
            return _mapper.Map<TListDto>(entity);
        }

        public virtual async Task<TListDto?> AddAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
            await _uow.CommitAsync();
            return _mapper.Map<TListDto>(entity);
        }

        public virtual async Task UpdateAsync(TUpdateDto updateDto)
        {
            var entity = _mapper.Map<TEntity>(updateDto);
            _repository.Update(entity);
            await _uow.CommitAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _repository.Update(entity);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _uow.CommitAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.ExistsAsync(predicate);
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
