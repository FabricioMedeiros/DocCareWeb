using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface IGenericService<TEntity, TCreateDto, TUpdateDto, TListDto>
        where TEntity : BaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TListDto : class
    {
        Task<bool> ValidateCreateDto(TCreateDto createDto);
        Task<bool> ValidateUpdateDto(TUpdateDto updateDto);
        Task<PagedResult<TListDto>> GetAllAsync(Dictionary<string, string>? filters, int? pageNumber = null, int? pageSize = null);
        Task<TListDto?> GetByIdAsync(int id);
        Task<TListDto?> AddAsync(TCreateDto createDto);
        Task UpdateAsync(TUpdateDto updateDto);
        Task DeleteAsync(int id);
    }
}
