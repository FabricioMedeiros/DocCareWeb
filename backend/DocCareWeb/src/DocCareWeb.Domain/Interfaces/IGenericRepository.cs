using DocCareWeb.Domain.Entities;
using System.Linq.Expressions;

namespace DocCareWeb.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<(IEnumerable<TEntity> Items, int TotalRecords)> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            int? skip = null,
            int? take = null);

        Task<TEntity?> GetByIdAsync(
            int id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        Task DeleteAsync(int id);
    }
}