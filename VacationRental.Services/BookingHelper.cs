using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Data.IRepository;
using VacationRental.Domain.Models;

namespace VacationRental.Services
{
    public class BookingHelper : IBookingHelper
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingHelper(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public bool CheckCanChange(int rentalId, int units, int preparationTime)
        {
            IEnumerable<BookingViewModel> bookings = _bookingRepository.GetByRentalId(rentalId);
            // Check if there are any booking in the provided booking dates
            foreach (var booking in bookings)
            {
                int count = AnyBookings(bookings, booking.Start, booking.End, preparationTime) - 1;
                if (count >= units)
                    return false;
            }

            return true;
        }

        private int AnyBookings(IEnumerable<BookingViewModel> bookings, DateTime startDate, DateTime endDate, int preparationTimeInDays)
        {
            int count = 0;

            DateTime date = startDate;
            while (date < endDate.AddDays(preparationTimeInDays))
            {
                int c = bookings.Where(b => b.Start <= date && date < b.End.AddDays(preparationTimeInDays)).Count();
                if (c > count) count = c;
                date = date.AddDays(1);
            }

            return count;
        }

        public bool CheckVacancy(int rentalId, DateTime start, DateTime end)
        {

            RentalViewModel rental = _rentalRepository.GetById(rentalId);
            IEnumerable<BookingViewModel> bookings = _bookingRepository.GetByRentalId(rentalId);

            int count = AnyBookings(bookings, start, end, rental.PreparationTimeInDays);
            if (count >= rental.Units)
                return false;

            return true;
        }
    }
}
