using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VacationRental.Repository.Entities;
using VacationRental.Repository.Repositories.Interfaces;

namespace VacationRental.Repository.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BookingsRepository : IBookingsRepository
    {
        private readonly IDictionary<int, BookingEntity> _bookings;

        public BookingsRepository(IDictionary<int, BookingEntity> bookings)
        {
            _bookings = bookings;
        }

        public BookingEntity GetBookingEntity(int bookingId)
        {
            BookingEntity bookingEntity;
            _bookings.TryGetValue(bookingId, out bookingEntity);

            return bookingEntity;
        }

        public List<BookingEntity> GetBookingEntities()
        {
            return _bookings.Values.ToList();
        }

        public int CreateBookingEntity(BookingEntity bookingEntity)
        {
            bookingEntity.Id = _bookings.Keys.Count + 1;
            _bookings.Add(bookingEntity.Id, bookingEntity);

            return bookingEntity.Id;
        }
    }
}
