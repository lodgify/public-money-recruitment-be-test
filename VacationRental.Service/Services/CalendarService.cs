using VacationRental.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace VacationRental.Application
{
    public class CalendarService : ICalendarService
    {
        private IBookingRepository _iBookingRepository;
        private IRentalRepository _iRentalRepository;
        
        public CalendarService(IBookingRepository iBookingRepository, IRentalRepository iRentalRepository)
        {
            _iBookingRepository = iBookingRepository;
            _iRentalRepository = iRentalRepository;
        }

        public GetCalendarResponse GetBooking(GetCalendarRequest request)
        {
            GetCalendarResponse response = new GetCalendarResponse();

            try
            { 
            if (request.numberOfnights < 0)
            {
                response.Message = "Nights must be positive";
                response.Success = false;
                return response;
            }

            if (_iRentalRepository.GetAll().Where(w=>w.Id == request.rentalId).Count() == 0)
            {
                response.Message = "Rental not found";
                response.Success = false;
                return response;
            }

            CalendarViewModel calendarViewModel = new CalendarViewModel
            {
                RentalId = request.rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < request.numberOfnights; i++)
            {
                CalendarDateViewModel calendarDateViewModel = new CalendarDateViewModel
                {
                    Date = request.bookingStartDate.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimesViewModel>()
                };

                int preparationDays = _iRentalRepository.GetById(request.rentalId).PreparationTimeInDays;

                foreach (var booking in _iBookingRepository.GetAll().Where(w=> w.Rental.Id == request.rentalId))
                {

                    if (calendarDateViewModel.Date >= booking.StartDate && calendarDateViewModel.Date < booking.StartDate.AddDays(booking.NumberOfNights))
                    {
                        calendarDateViewModel.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = 1 });
                    }

                    if (calendarDateViewModel.Date >= booking.StartDate.AddDays(booking.NumberOfNights) && calendarDateViewModel.Date < booking.StartDate.AddDays(booking.NumberOfNights + preparationDays))
                    {
                        calendarDateViewModel.PreparationTimes.Add(new PreparationTimesViewModel { Unit = 1 });
                    }
                }

                calendarViewModel.Dates.Add(calendarDateViewModel);
            }

            response.CalendarViewModel = calendarViewModel;
            response.Success = true;

            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Message = exception.Message;
            }

            return response;
        }
    }
}
