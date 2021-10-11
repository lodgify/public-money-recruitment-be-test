using System.Threading.Tasks;
using VacationRental.Domain.Base;

namespace VacationRental.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IAsyncRepository<T> AsyncRepository<T>() where T : BaseEntity;
        Task RollBack<T>() where T : BaseEntity;
    }
}