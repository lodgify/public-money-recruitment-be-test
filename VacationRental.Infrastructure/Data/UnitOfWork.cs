using System.Threading.Tasks;
using VacationRental.Domain.Base;
using VacationRental.Domain.Interfaces;
using VacationRental.Infrastructure.Data.Repositories;
using VacationRental.Infrastructure.DataBase;

namespace VacationRental.Infrastructure.Data
{

    public class UnitOfWork : IUnitOfWork
    {
        private InMemoryDatabase database = new InMemoryDatabase();
        private EFContext _dbContext { get; }

        public UnitOfWork()
        {
            _dbContext = database.createContext();
        }

        public IAsyncRepository<T> AsyncRepository<T>() where T : BaseEntity
        {
            return new RepositoryBase<T>(_dbContext);
        }

        public Task RollBack<T>() where T : BaseEntity
        {
            RepositoryBase<T> repository = new RepositoryBase<T>(_dbContext);
            return Task.FromResult(repository);
        }

        public Task<int> SaveChangesAsync()
        {
            database.commit(_dbContext.ToDatabase());
            return Task.FromResult(0);
        }
    }
}