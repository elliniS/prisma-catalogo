using System.Linq.Expressions;

namespace PrismaCatalogo.Api.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync<TS>(Expression<Func<T, TS>> select);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetListAsync<TS>(Expression<Func<T, bool>> predicate, Expression<Func<T, TS>> select);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
