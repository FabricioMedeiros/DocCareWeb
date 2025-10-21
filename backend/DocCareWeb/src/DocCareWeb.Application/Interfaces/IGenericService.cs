using DocCareWeb.Domain.Entities;
using System.Linq.Expressions;

namespace DocCareWeb.Application.Interfaces
{
    public interface IGenericService<TEntity, TCreateDto, TUpdateDto, TListDto>
        where TEntity : BaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TListDto : class
    {
        Task<PagedResult<TListDto>> GetAllAsync(
           Dictionary<string, string>? filters,
           int? pageNumber = null,
           int? pageSize = null,
           Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<TListDto?> GetByIdAsync(
            int id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<TEntity?> GetByIdAsync(int id, bool returnEntity);

        Task<TListDto?> AddAsync(TCreateDto createDto, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<TListDto?> AddAsync(TEntity entity, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task UpdateAsync(TUpdateDto updateDto);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}