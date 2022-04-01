using System;
using System.Collections.Generic;
using VacationRental.Domain.Bookings;

namespace VacationRental.Application.Common.Services.BookingSearchService
{
    public class GetBookingsByRangeOfTimeDTO
    {
        public IEnumerable<BookingModel> Bookings { get; set; } 
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int PreparationTime { get; set; } = 0;
    }
}
