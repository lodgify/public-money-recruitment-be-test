using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        int Add(T entity);
        T Update(T entity);
        T Get(int id);
        bool Exists(int Id);
        IEnumerable<T> GetAll(Func<T, bool> predicate);
        bool DeleteAll(Func<T, bool> predicate);
    }
}