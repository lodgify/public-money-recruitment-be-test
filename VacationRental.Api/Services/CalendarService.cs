using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingService _bookingService;
        private readonly IRentalService _rentalService;

        public CalendarService(
            IBookingService bookingService,
            IRentalService rentalService)
        {
            _bookingService = bookingService;
            _rentalService = rentalService;
        }

        public CalendarViewModel Get(CalendarBindingModel model)
        {
            if (model == null)
                throw new ApplicationException("The supplied calendar model is not appropriate!");

            if (model.RentalId <= 0)
                throw new ApplicationException("Rental Id must be greater than zero!");

            if (model.Start <= DateTime.MinValue)
                throw new ApplicationException("Start date must be greater than minimum date!");

            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be greater than zero!");

            RentalViewModel rental = _rentalService.Get(model.RentalId);

            if (rental == null)
                throw new ApplicationException($"Rental not found with rentalId:{model.RentalId}!");

            CalendarViewModel result = new CalendarViewModel
            {
                RentalId = model.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (int i = 0; i < model.Nights; i++)
            {
                CalendarDateViewModel date = new CalendarDateViewModel
                {
                    Date = model.Start.Date.AddDays(i)
                };

                date.Bookings = _bookingService.GetBookingsByDate(model.RentalId, date.Date).ToList();
                date.Preparations = _bookingService.GetBookingsWithPreparationsByDate(model.RentalId, rental.PreparationTimeInDays, date.Date).ToList();

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
