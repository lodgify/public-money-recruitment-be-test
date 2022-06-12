using System;
using System.Collections.Generic;
using VacationRental.Data.IRepository;
using VacationRental.Domain.Models;
using VacationRental.Services.IServices;
using VacationRental.Domain.Extensions;

namespace VacationRental.Services.Services
{
    public class CalenderService : ICalenderService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public CalenderService(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if(nights < 0)
            {
                throw new ApplicationException("Nights must be positive");
            }

            IEnumerable<BookingViewModel> booking = _bookingRepository.GetByRentalId(rentalId);
            CalendarViewModel result = booking.GenerateCalendar(rentalId, start, nights);

            return result;
        }

        public CalendarViewModel GetVacationrental(int rentalId, DateTime start, int nights)
        {
            RentalViewModel rental = _rentalRepository.GetById(rentalId);
            IEnumerable<BookingViewModel> bookings = _bookingRepository.GetByRentalId(rentalId);

            CalendarViewModel result = rental.GenerateCalender(bookings, rentalId, start, nights);

            return result;
        }
    }
}
