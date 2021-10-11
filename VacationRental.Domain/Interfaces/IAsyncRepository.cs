using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Base;

namespace VacationRental.Domain.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        Task<T> GetAsync(int id);

        Task<List<T>> ListAsync(Func<IBaseEntity, bool> expression);
    }
}