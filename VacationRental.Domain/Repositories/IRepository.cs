using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Domain.Repositories
{
    public interface IRepository<T>
    {
        T Create(T entity);
        T GetById(int id);
    }
}
