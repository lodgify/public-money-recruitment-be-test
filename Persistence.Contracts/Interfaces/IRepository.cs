using System.Linq.Expressions;

namespace Persistence.Contracts.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(int id);
    T GetById(int id);
    Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
    T FindByCondition(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    Task<bool> SaveChangesAsync();
}