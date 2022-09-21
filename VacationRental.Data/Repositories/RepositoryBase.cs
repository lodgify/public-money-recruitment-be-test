using VacationRental.Data.Model.Abstractions;
using VacationRental.Data.Repositories.Abstractions;

namespace VacationRental.Data.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : IDataEntity
{
    protected static int _lastIdentity;
    protected static Dictionary<int, T> DataSource { get; set; } = new Dictionary<int, T>();

    protected static int NextIdentity()
    {
        return ++_lastIdentity;
    }

    protected static void RevertIdentity()
    {
        _lastIdentity--;
    }

    public virtual T Get(int id)
    {
        if (DataSource.TryGetValue(id, out var result))
            return result!;

        throw new ApplicationException($"{typeof(T).Name} not found");
    }

    public virtual ICollection<T> GetAll()
    {
        return DataSource.Values.ToList();
    }

    public virtual T Add(T value)
    {
        var id = NextIdentity();
        if (DataSource.TryAdd(id, value))
        {
            value.Id = id;
            return value;
        }

        RevertIdentity();
        throw new InvalidOperationException();
    }

    public T Update(T value)
    {
        if (DataSource.TryGetValue(value.Id, out _))
        {
            DataSource[value.Id] = value;
        }

        throw new ApplicationException($"{typeof(T).Name} not found");
    }

    public void Delete(T value)
    {
        if (!DataSource.TryGetValue(value.Id, out _))
        {
            throw new ApplicationException($"{typeof(T).Name} not found");
        }

        DataSource.Remove(value.Id);
    }

    public void Delete(int id)
    {
        if (!DataSource.TryGetValue(id, out _))
        {
            throw new ApplicationException($"{typeof(T).Name} not found");
        }

        DataSource.Remove(id);
    }
}