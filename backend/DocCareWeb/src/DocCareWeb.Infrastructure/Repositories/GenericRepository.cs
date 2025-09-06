using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using DocCareWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DocCareWeb.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        protected IQueryable<TEntity> ApplyIncludes(
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            return includes != null ? includes(query) : query;
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalRecords)> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            int? skip = null,
            int? take = null)
        {
            IQueryable<TEntity> query = ApplyIncludes(includes);

            if (filter != null)
                query = query.Where(filter);

            int totalRecords = await query.CountAsync();

            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);

            var items = await query.ToListAsync();
            return (items, totalRecords);
        }

        public async Task<TEntity?> GetByIdAsync(
            int id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            IQueryable<TEntity> query = ApplyIncludes(includes);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null
                ? await _context.Set<TEntity>().CountAsync()
                : await _context.Set<TEntity>().CountAsync(predicate);
        }
    }
}
