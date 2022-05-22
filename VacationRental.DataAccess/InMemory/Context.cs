using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Entities;

namespace VacationRental.DataAccess.InMemory
{
    public class Context
    {
        public IDictionary<int,Rental> rentals= new Dictionary<int,Rental>();

        public IDictionary<int, Booking> bookings = new Dictionary<int, Booking>();
    }
}
