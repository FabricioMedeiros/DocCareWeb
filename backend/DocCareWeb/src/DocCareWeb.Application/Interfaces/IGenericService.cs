using DocCareWeb.Domain.Entities;
using System.Linq.Expressions;

namespace DocCareWeb.Application.Interfaces
{
    public interface IGenericService<TEntity, TCreateDto, TUpdateDto, TDto>
        where TEntity : BaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TDto : class
    {
        Task<bool> ValidateCreateDto(TCreateDto createDto);
        Task<bool> ValidateUpdateDto(TUpdateDto updateDto);
        Task<PagedResult<TDto>> GetAllAsync(Expression<Func<TDto, bool>>? filter = null, int? pageNumber = null, int? pageSize = null);
        Task<TDto?> GetByIdAsync(int id);
        Task AddAsync(TCreateDto createDto);
        Task UpdateAsync(TUpdateDto updateDto);
        Task DeleteAsync(int id);
    }
}
