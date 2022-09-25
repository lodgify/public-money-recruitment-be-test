using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            if (model.Units <= 0)
                throw new ApplicationException("Units must be positive");
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("PreparationTimeInDays cannot be negative");

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }


        [HttpPut]
        public void Put(RentalEditModel model)
        {
            // make sure input is valid
            if (model.Units <= 0)
                throw new ApplicationException("Units must be positive");
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("PreparationTimeInDays cannot be negative");


            foreach (var rental in _rentals)
            {
                if (model.Id == rental.Value.Id)
                {
                    // check if number of Units has been changed
                    if (model.Units != rental.Value.Units)
                    {
                        int mostBookingsAtOnce = 0;
                        // loops through all bookings
                        foreach (var booking in _bookings.Values)
                        {
                            // checks if the booking appplies to this rental
                            if (booking.RentalId == model.Id)
                            {
                                // runs through each date in this booking
                                for (DateTime day = booking.Start; day <= booking.Start.AddDays(booking.Nights + rental.Value.PreparationTimeInDays); day = day.AddDays(1))
                                {
                                    int bookingCounter = 1;
                                    // checks all other bookings to see if they go through this date. If so, increase bookingCounter
                                    foreach (var secondBooking in _bookings.Values)
                                    {
                                        if (secondBooking.RentalId == model.Id && secondBooking.Start <= day && secondBooking.Start.AddDays(secondBooking.Nights + rental.Value.PreparationTimeInDays) >= day)
                                        {
                                            bookingCounter++;
                                        }
                                    }
                                    if (bookingCounter > mostBookingsAtOnce)
                                    {
                                        mostBookingsAtOnce = bookingCounter;
                                    }
                                    bookingCounter = 0;
                                }
                            }
                        }
                        // if all units are unavailiable at some point
                        if (mostBookingsAtOnce >= rental.Value.Units)
                            throw new ApplicationException("Removing Units is not possible since all are needed to fulfil bookings");
                        rental.Value.Units = model.Units;
                    }

                    // check if preparation days has been changed
                    if (model.PreparationTimeInDays != rental.Value.PreparationTimeInDays)
                    {
                        int mostBookingsAtOnce = 0;
                        // do the same thing as above, to make sure that if PreparationTime is increased there will be no clashes
                        foreach (var booking in _bookings.Values)
                        {
                            if (booking.RentalId == model.Id)
                            {
                                for (DateTime day = booking.Start; day <= booking.Start.AddDays(booking.Nights + model.PreparationTimeInDays); day = day.AddDays(1))
                                {
                                    int bookingCounter = 1;
                                    foreach (var secondBooking in _bookings.Values)
                                    {
                                        if (secondBooking.RentalId == model.Id && secondBooking.Start <= day && secondBooking.Start.AddDays(secondBooking.Nights + model.PreparationTimeInDays) >= day)
                                        {
                                            bookingCounter++;
                                        }
                                    }
                                    if (bookingCounter > mostBookingsAtOnce)
                                    {
                                        mostBookingsAtOnce = bookingCounter;
                                    }
                                    bookingCounter = 0;
                                }
                            }
                        }
                        if (mostBookingsAtOnce > rental.Value.Units)
                            throw new ApplicationException("Increasing PreparationTime is not possible since it would cause clashes between bookings");
                        rental.Value.PreparationTimeInDays = model.PreparationTimeInDays;
                        break;
                    }
                }
            }
        }
    }
}