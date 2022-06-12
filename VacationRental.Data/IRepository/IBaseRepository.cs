using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Data.IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        bool Delete(int id);
    }
}
