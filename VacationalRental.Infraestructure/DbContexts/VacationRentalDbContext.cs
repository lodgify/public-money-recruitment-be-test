using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationalRental.Infrastructure.DbContexts
{
    public class VacationRentalDbContext : DbContext
    {
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options) : base(options)
        {

        }
    }
}
