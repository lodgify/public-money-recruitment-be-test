using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Infrastructure.Models;

namespace VacationRental.Infrastructure.Repositories
{
    public class BookingRepository : IBookingsRepository
    {
        private readonly IDictionary<int, Booking> _bookings;

        public BookingRepository(IDictionary<int, Booking> bookings)
        {
            _bookings = bookings;
        }
        public int Add(Booking booking)
        {
            int key = _bookings.Keys.Count + 1;
            _bookings.Add(key, new Booking
            {
                Id = key,
                Nights = booking.Nights,
                RentalId = booking.RentalId,
                Start = booking.Start
            });

            return key;
        }

        public bool DeleteAll(Func<Booking, bool> predicate)
        {
            try
            {
                var bookings = GetAll(predicate);
                foreach (var booking in bookings)
                    _bookings.Remove(booking.Id);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Exists(int Id)
        {
            return _bookings.ContainsKey(Id);
        }

        public Booking Get(int id)
        {
            if (_bookings.ContainsKey(id))
                return _bookings[id];
            else
                return null;
        }

        public IEnumerable<Booking> GetAll(Func<Booking, bool> predicate)
        {
            var bookings = _bookings
                .Select(booking => booking.Value)
                .Where(predicate);

            return bookings;
        }

        public IEnumerable<Booking> GetConflictingBookings(DateTime start, int nights, int preparationTime, int rentalId)
        {
            var rentalBookings = _bookings.Values.Where(booking => booking.RentalId == rentalId);

            var conflictingBookings = rentalBookings.Where(booking =>
            (booking.Start <= start.Date && booking.Start.AddDays(booking.Nights + preparationTime) > start.Date)
                        || (booking.Start < start.AddDays(nights + preparationTime) && booking.Start.AddDays(booking.Nights + preparationTime)
                        >= start.AddDays(nights + preparationTime))
                        || (booking.Start > start && booking.Start.AddDays(booking.Nights + preparationTime) < start.AddDays(nights + preparationTime)));

            return conflictingBookings;
        }

        public Booking Update(Booking entity)
        {
            throw new NotImplementedException();
        }
    }
}
