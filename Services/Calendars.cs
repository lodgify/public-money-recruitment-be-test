using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Models.Booking;
using VacationRental.Models.Calendar;
using VacationRental.Models.Rental;

namespace VacationRental.Services
{
    public class Calendars
    {
        public static CalendarViewModel getAvailability(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (Services.Rentals.getRentalById(rentalId).Id == null)
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()                
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                List<BookingViewModel> bookings = DAL.Bookings.getBookings();
                foreach (var booking in bookings)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }



        private List<CalendarDateViewModel> bookingsDate(int rentalId, DateTime start, int nights  )
        {
            var result  = new List<CalendarDateViewModel>();
            RentalViewModel rental = Rentals.getRentalById(rentalId);
            List<BookingViewModel> bookingsRental = DAL.Bookings.getBookingsByRentalId(rentalId);
            for (var i = 0; i < nights; i++)
            {
                CalendarDateViewModel day = new CalendarDateViewModel();
                day.Date = start.AddDays(i);
                


                day.Bookings = new List<CalendarBookingViewModel>();
                
                day.PreparationTimes = new List<CalendarPreparationTime>();

                List<BookingViewModel> bookingsDay = bookingsRental.FindAll(x => x.RentalId == rentalId && x.Start <= day.Date && x.Start.AddDays(x.Nights) > day.Date);

                foreach (var book in bookingsDay)
                {
                    CalendarBookingViewModel item = new CalendarBookingViewModel();
                    item.Id = book.Id;
                    item.Unit = book.Unit;
                    day.Bookings.Add(item);

                    CalendarPreparationTime preparation = new CalendarPreparationTime();
                    preparation.Unit = bookingsDay.Count - rental.PreparationTimeInDays;
                    day.PreparationTimes.Add(preparation);

                }
              

                result.Add(day);
            }
            return result;
    }
}
