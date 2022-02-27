using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Data;
using VacationRental.Api.Helpers;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IRentalService _rentalService;

        public BookingService(
            IDictionary<int, Booking> bookings,
            IRentalService rentalService)
        {
            _bookings = bookings;
            _rentalService = rentalService;
        }

        public ResourceIdViewModel Create(BookingBindingModel model)
        {
            if (model == null)
                throw new ApplicationException("The supplied booking is not appropriate!");

            if (model.RentalId <= 0)
                throw new ApplicationException("Rental Id must be greater than zero!");

            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be greater than zero!");

            if (model.Start <= DateTime.MinValue)
                throw new ApplicationException("Start date must be greater than minimum date!");

            RentalViewModel rental = _rentalService.Get(model.RentalId);

            int availableUnit = GetAvailableUnit(rental, model, model.Nights);

            return CreateBooking(model, availableUnit);
        }

        public BookingViewModel Get(int bookingId)
        {
            if (bookingId <= 0)
                throw new ApplicationException("Booking Id must be greater than zero!");

            Booking booking = _bookings.Values.FirstOrDefault(r => r.Id == bookingId);

            if (booking == null)
                throw new ApplicationException($"Booking not found with bookingId: {bookingId}!");

            return new BookingViewModel()
            {
                Id = booking.Id,
                Nights = booking.Nights,
                RentalId = booking.RentalId,
                Start = booking.Start
            };
        }

        public IEnumerable<CalendarBookingViewModel> GetBookingsByDate(int rentalId, DateTime date)
        {
            return _bookings.Values.Where(x => x.RentalId == rentalId
                                                                && (x.Start <= date.Date
                                                                && x.Start.AddDays(x.Nights) > date.Date))
                .Select(x => new CalendarBookingViewModel()
                {
                    Id = x.Id,
                    Unit = x.Unit
                });
        }

        public IEnumerable<CalendarPreparationViewModel> GetPreparationsByDate(int rentalId, int preparationTime, DateTime date)
        {
            return _bookings.Values.Where(x => x.RentalId == rentalId
                                                            && x.Start.AddDays(x.Nights) <= date.Date
                                                            && x.Start.AddDays(x.Nights + preparationTime) > date.Date)
                .Select(x => new CalendarPreparationViewModel()
                {
                    Unit = x.Unit
                });
        }

        private int GetAvailableUnit(RentalViewModel rentalModel, BookingBindingModel bookingModel, int nights)
        {
            List<int> bookedUnits = new List<int>();

            List<Booking> bookings = GetBookingsByDate(rentalModel,
                bookingModel.Start,
                bookingModel.Start.AddDays(bookingModel.Nights + rentalModel.PreparationTimeInDays));

            for (int i = 0; i < nights; i++)
            {
                foreach (Booking booking in bookings)
                {
                    DateTime bookingEndDate = booking.Start.AddDays(booking.Nights + rentalModel.PreparationTimeInDays);

                    if ((booking.Start <= bookingModel.Start.Date && bookingEndDate > bookingModel.Start.Date)
                        || (booking.Start < bookingModel.Start.AddDays(nights) && bookingEndDate >= bookingModel.Start.AddDays(nights))
                        || (booking.Start > bookingModel.Start && bookingEndDate < bookingModel.Start.AddDays(nights)))
                    {
                        // double addition prevention
                        if (!bookedUnits.Contains(booking.Unit))
                        {
                            bookedUnits.Add(booking.Unit);
                        }
                    }
                }

                if (bookedUnits.Count > rentalModel.Units)
                    throw new ApplicationException("No available unit could found for this process");
            }

            List<int> allUnits = UnitHelper.GetUnits(rentalModel.Units);
            int newSelectedUnit = allUnits.FirstOrDefault(x => !bookedUnits.Contains(x));

            if (newSelectedUnit == default)
                throw new ApplicationException("No available unit could found for this process");

            return newSelectedUnit;
        }

        private List<Booking> GetBookingsByDate(RentalViewModel rental, DateTime startDate, DateTime endDate)
        {
            return _bookings.Values.Where(x => x.RentalId == rental.Id &&
                                                                        (x.Start <= startDate.Date && x.Start.AddDays(x.Nights + rental.PreparationTimeInDays) > startDate.Date)
                                                                        || (x.Start < endDate.Date && x.Start.AddDays(x.Nights + rental.PreparationTimeInDays) >= endDate.Date)
                                                                        || (x.Start > startDate.Date && x.Start.AddDays(x.Nights + rental.PreparationTimeInDays) < endDate.Date))
                .ToList();
        }

        private ResourceIdViewModel CreateBooking(BookingBindingModel model, int availableUnit)
        {
            ResourceIdViewModel key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new Booking
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = availableUnit
            });
            return key;
        }
    }
}
