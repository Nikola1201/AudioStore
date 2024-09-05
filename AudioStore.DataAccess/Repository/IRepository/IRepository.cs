using System.Linq.Expressions;


namespace AudioStore.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
