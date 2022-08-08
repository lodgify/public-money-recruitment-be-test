namespace VacationRental.Api.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        void Add(int key, T item);
        bool HasValue(int id);
        int Count { get; }
    }
}
