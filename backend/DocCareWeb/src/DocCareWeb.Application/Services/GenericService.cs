using AutoMapper;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Utils;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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

            int? skip = null;
            int? take = null;

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                skip = (pageNumber.Value - 1) * pageSize.Value;
                take = pageSize.Value;
            }

            var (items, totalRecords) = await _repository.GetAllAsync(
                filter: filterExpression,
                includes: includes,
                skip: skip,
                take: take);

            return new PagedResult<TListDto>
            {
                Page = pageNumber ?? 1,
                PageSize = pageSize ?? totalRecords, 
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

        public virtual async Task<TListDto?> AddAsync(TEntity entity,
                                                      Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            var fullEntity = await _repository.GetByIdAsync(entity.Id, includes);
            return _mapper.Map<TListDto>(fullEntity);
        }

        public virtual async Task<TListDto?> AddAsync(TCreateDto createDto,
                                                      Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            var fullEntity = await _repository.GetByIdAsync(entity.Id, includes);
            return _mapper.Map<TListDto>(fullEntity);
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

            try
            {
                await _uow.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;

                if (message.Contains("REFERENCE constraint"))
                   Notify("Não é possível excluir, existem registros vinculados.");
                else
                   Notify("Erro inesperado ao processar a operação.");
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.ExistsAsync(predicate);
        }

        protected static Expression<Func<TEntity, bool>> ApplyFilters(Dictionary<string, string>? filters)
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
