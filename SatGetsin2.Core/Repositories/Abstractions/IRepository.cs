using SatGetsin2.Core.Entities.Base;
using System.Linq.Expressions;

namespace SatGetsin2.Core.Repositories.Abstractions
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entities);
        Task<int> SaveAsync();
    }
}
