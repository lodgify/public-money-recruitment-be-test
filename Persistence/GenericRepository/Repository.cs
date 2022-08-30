using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts.Interfaces;

namespace Persistence.GenericRepository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IApplicationDbContext _context;
    private readonly DbSet<T> _entities;

    public Repository(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _entities = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public T GetById(int id)
    {
        return _entities.Find(id);
    }

    public T FindByCondition(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }

    public void Add(T entity)
    {
        _entities.Add(entity);
    }

    public void Update(T entity)
    {
        _entities.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }
}