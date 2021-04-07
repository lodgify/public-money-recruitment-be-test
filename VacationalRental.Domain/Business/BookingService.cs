using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;

namespace VacationalRental.Domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IRentalsRepository _rentalsRepository;

        public BookingService(
            IBookingsRepository bookingsRepository,
            IRentalsRepository rentalsRepository)
        {
            _bookingsRepository = bookingsRepository;
            _rentalsRepository = rentalsRepository;
        }

        public async Task<(InsertNewBookingStatus, int)> InsertNewBooking(BookingEntity bookingEntity)
        {
            var rentalUnits = await _rentalsRepository.GetRentalUnits(bookingEntity.RentalId);
            var bookings = await _bookingsRepository.GetBookinByRentalId(bookingEntity.RentalId);

            for (var i = 0; i < bookingEntity.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookings)
                {
                    if (booking.RentalId == bookingEntity.RentalId
                        && (booking.Start <= bookingEntity.Start.Date && booking.Start.AddDays(booking.Nights) > bookingEntity.Start.Date)
                        || (booking.Start < bookingEntity.Start.AddDays(bookingEntity.Nights) && booking.Start.AddDays(booking.Nights) >= bookingEntity.Start.AddDays(bookingEntity.Nights))
                        || (booking.Start > bookingEntity.Start && booking.Start.AddDays(booking.Nights) < bookingEntity.Start.AddDays(bookingEntity.Nights)))
                    {
                        count++;
                    }
                }

                if (count >= rentalUnits)
                    return (InsertNewBookingStatus.NotAvailable, 0);
            }

            var lastUnit = await _bookingsRepository.GetLastUnit(bookingEntity.RentalId);
            lastUnit++;
            bookingEntity.Unit = lastUnit;

            var bookingId = await _bookingsRepository.InsertBooking(bookingEntity);
            if (bookingId <= 0)
                return (InsertNewBookingStatus.InsertDbNoRowsAffected, 0);

            return (InsertNewBookingStatus.OK, bookingId);
        }

        public async Task<BookingEntity> GetBookingById(int bookingId)
        {
            return await _bookingsRepository.GetBookingById(bookingId);
        }

        public async Task<bool> BookingExists(int bookingId)
        {
            return await _bookingsRepository.BookingExists(bookingId);
        }
    }
}
