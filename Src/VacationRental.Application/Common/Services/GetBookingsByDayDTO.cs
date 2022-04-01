using System;
using System.Collections.Generic;
using VacationRental.Domain.Bookings;

namespace VacationRental.Application.Common.Services
{
    public class GetBookingsByDayDTO
    {
        public IEnumerable<BookingModel> Bookings { get; set; } 
        public DateTime Day { get; set; }
        public int PreparationTime { get; set; } = 0;
    }
}