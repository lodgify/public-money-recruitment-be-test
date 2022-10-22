using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Infrastructure.Contracts
{
    public interface IBaseRepository<TEntity> 
        where TEntity : class
    {
        Task<IDictionary<int, TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<ResourceIdViewModel> DeleteAsync(int id);
        Task<ResourceIdViewModel> AddAsync(TEntity entityViewModel);
        Task<ResourceIdViewModel> UpdateAsync(TEntity entityViewModel);
    }
}
