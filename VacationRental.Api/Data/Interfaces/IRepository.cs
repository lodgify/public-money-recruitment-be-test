namespace VacationRental.Api.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        int Insert(T item);
        bool HasValue(int id);
    }
}
