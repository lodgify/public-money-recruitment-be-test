using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Data.Entities;
using VacationRental.Data;
using VacationRental.Repository.Interfaces;

namespace VacationRental.Repository.Implementations
{
    public class BookingRepository : BaseRepository<Booking, DataContext>, IBookingRepository
    {
        public BookingRepository(DataContext context) : base(context)
        {

        }
    }
}
