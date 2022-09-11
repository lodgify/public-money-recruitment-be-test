using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Data.Factory
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql("User ID=user_rental;Password=rental123;Server=62.171.149.43;Port=8085;Database=rentaldb;Integrated Security=true;Pooling=true;");
            return new DataContext(optionsBuilder.Options);
        }
    }
}
