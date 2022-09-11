using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Data;
using VacationRental.Data.Entities;
using VacationRental.Repository.Interfaces;

namespace VacationRental.Repository.Implementations
{
    public class RentalRepository : BaseRepository<Rental, DataContext>, IRentalRepository
    {
        public RentalRepository(DataContext context) : base(context)
        {

        }
    }
}
