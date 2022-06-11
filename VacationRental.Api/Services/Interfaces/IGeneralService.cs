namespace VacationRental.Api.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IGeneralService<T>
    {
        IDictionary<int, T> Get();
        T Get(int id);
        void Add(int key, T entity);
        void Update(int key, T entity);
        int GenerateId();
    }
}
