using DocCareWeb.Domain.Entities;
using System.Linq.Expressions;

namespace DocCareWeb.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<(IEnumerable<TEntity?> Items, int TotalRecords)> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity?> GetByIdAsync(
            int id,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}