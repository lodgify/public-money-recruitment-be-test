using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VacationRental.Domain.Primitives;

namespace VacationRental.Application.Contracts.Persistence
{
    public interface IRepository<T> where T : BaseDomainModel
    {
        IReadOnlyList<T> GetAll();

        T? GetById(int id);

        T Add(T entity);
        
        T? Update(T entity);

        void Delete(T entity);
    }
}
