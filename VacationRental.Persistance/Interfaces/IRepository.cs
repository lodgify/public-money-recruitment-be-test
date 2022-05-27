using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Persistance.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Insert(T entity);
        Task InsertAsync(T entity);
        void Update(T entity);
    }
}
