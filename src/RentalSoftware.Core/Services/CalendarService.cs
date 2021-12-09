
using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Contracts.Response;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingService _bookingService;
        private readonly IRentalService _rentalService;

        public CalendarService(IBookingService bookingService, IRentalService rentalService)
        {
            _bookingService = bookingService;
            _rentalService = rentalService;
        }

        public async Task<GetCalendarResponse> GetCalendar(GetCalendarRequest request)
        {
            GetCalendarResponse response = new GetCalendarResponse();

            try
            {
                if (request.NumberOfNights < 0)
                {
                    response.Message = "Nights must be positive";
                    response.Succeeded = false;
                    return response;
                }

                var rentals = await _rentalService.GetAll();

                if (rentals.Where(w => w.Id == request.RentalId).Count() == 0)
                {
                    response.Message = "Not Found";
                    response.Succeeded = false;
                    return response;
                }

                CalendarViewModel calendarViewModel = new CalendarViewModel
                {
                    RentalId = request.RentalId,
                    Dates = new List<CalendarDateViewModel>()
                };

                for (var i = 0; i < request.NumberOfNights; i++)
                {
                    CalendarDateViewModel calendarDateViewModel = new CalendarDateViewModel
                    {
                        Date = request.BookingStartDate.Date.AddDays(i),
                        Bookings = new List<CalendarBooking>(),
                        PreparationTimes = new List<PreparationTime>()
                    };

                    var rental = await _rentalService.GetByRentalId(request.RentalId);
                    var preparationDays = rental.PreparationTime;

                    var bookings = await _bookingService.GetAll();
                    bookings = bookings.Where(x => x.RentalId == request.RentalId);

                    List<BookingsAndRentals> BookingsAndRentals = new List<BookingsAndRentals>();

                    foreach (var booking in bookings)
                    {
                        BookingsAndRentals.Add(new BookingsAndRentals()
                        {
                            Id = booking.Id,
                            NumberOfNights = booking.Nights,
                            StartDate = booking.Start,
                            Rental = await _rentalService.GetByRentalId(request.RentalId),
                        });
                    }

                    foreach (var booking in BookingsAndRentals)
                    {

                        if (calendarDateViewModel.Date >= booking.StartDate && calendarDateViewModel.Date < booking.StartDate.AddDays(booking.NumberOfNights))
                        {
                            calendarDateViewModel.Bookings.Add(new CalendarBooking { Id = booking.Id, Unit = 1 });
                        }

                        if (calendarDateViewModel.Date >= booking.StartDate.AddDays(booking.NumberOfNights) && calendarDateViewModel.Date < booking.StartDate.AddDays(booking.NumberOfNights + preparationDays))
                        {
                            calendarDateViewModel.PreparationTimes.Add(new PreparationTime { Unit = 1 });
                        }
                    }

                    calendarViewModel.Dates.Add(calendarDateViewModel);
                }

                response.CalendarViewModel = calendarViewModel;
                response.Succeeded = true;

            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }


}
