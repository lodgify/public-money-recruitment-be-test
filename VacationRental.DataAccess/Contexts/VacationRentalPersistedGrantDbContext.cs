using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace VacationRental.DataAccess.Contexts
{
    public class VacationRentalPersistedGrantDbContext : PersistedGrantDbContext<VacationRentalPersistedGrantDbContext>
    {
        public VacationRentalPersistedGrantDbContext(DbContextOptions<VacationRentalPersistedGrantDbContext> options, OperationalStoreOptions storeOptions) 
            : base(options, storeOptions)
        {
        }
    }
}
