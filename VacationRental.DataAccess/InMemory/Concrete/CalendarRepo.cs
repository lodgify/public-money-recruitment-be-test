using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.DataAccess.InMemory.Abstract;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.DataAccess.InMemory.Concrete
{
    public class CalendarRepo : ICalendarRepo
    {
        private readonly Context context;
        public CalendarRepo(Context context)
        {
            this.context = context;
        }
        public List<List<KeyValuePair<int, Booking>>> GetBookings(CalendarDto dto)
        {
            if (!context.rentals.ContainsKey(dto.rentalId))
            {
                return null;
            }

            List<List<KeyValuePair<int, Booking>>> lists = new List<List<KeyValuePair<int, Booking>>>();

            for (var i = 0; i < dto.nights; i++)
            {
                var date = dto.start.Date.AddDays(i);

                var preparationTime = context.rentals
                                      .Where(r => r.Value.Id == dto.rentalId)
                                      .Select(r=>r.Value.PreparationTimeInDays).First();

                var book = context.bookings.Where(b => b.Value.RentalId == dto.rentalId
                           && b.Value.Start <= date
                           && b.Value.Start.AddDays(b.Value.Nights-preparationTime) > date)
                           .ToList();

                lists.Add(book);
            }

            return lists;
        }
    }
}
