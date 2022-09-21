using VacationRental.Data.Model.Abstractions;

namespace VacationRental.Data.Repositories.Abstractions;

public interface IRepositoryBase<T> 
    where T : IDataEntity
{
    public T Add(T value);

    public T Get(int id);

    public ICollection<T> GetAll();
}
