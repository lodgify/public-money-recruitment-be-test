using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace VacationRental.DataAccess.Contexts
{
    public class VacationRentalConfigurationDbContext : ConfigurationDbContext<VacationRentalConfigurationDbContext>
    {
        public VacationRentalConfigurationDbContext(DbContextOptions<VacationRentalConfigurationDbContext> options, ConfigurationStoreOptions storeOptions) 
            : base(options, storeOptions)
        { }
    }
}
