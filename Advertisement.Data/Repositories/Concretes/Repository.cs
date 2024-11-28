using Advertisement.Core.Entities.Base;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Advertisement.Data.Repositories.Concretes
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            _context.RemoveRange(entities);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsQueryable();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
