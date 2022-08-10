namespace VacationRental.Api.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        int Add(T item);
        bool HasValue(int id);
    }
}
