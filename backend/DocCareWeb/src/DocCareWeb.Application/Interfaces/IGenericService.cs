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
            params Expression<Func<TEntity, object>>[] includes);

        Task<TListDto?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity?> GetByIdAsync(int id, bool returnEntity);

        Task<TListDto?> AddAsync(TCreateDto createDto);

        Task<TListDto?> AddAsync(TEntity entity);

        Task UpdateAsync(TUpdateDto updateDto);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);
    }
}