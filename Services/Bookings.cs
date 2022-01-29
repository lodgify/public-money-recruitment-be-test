using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Models;
using VacationRental.Models.Booking;
using VacationRental.Models.Rental;

namespace VacationRental.Services
{
    public class Bookings
    {
        public static BookingViewModel getBookingById(int bookingId)
        {
            return VacationRental.DAL.Bookings.getBookingsById(bookingId);
        }

        public static ResourceIdViewModel insert(BookingBindingModel model)
        {
            RentalViewModel rentalInfo = Rentals.getRentalById(model.RentalId);
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            if (rentalInfo.Id == null)
                throw new ApplicationException("Rental not found");

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                List<BookingViewModel> bookings = VacationRental.DAL.Bookings.getBookings();
            
                var unitStatus = bookings.FindAll(x => x.RentalId == model.RentalId && x.Unit ==model.Unit && 
                                              (x.Start <= model.Start.Date && x.Start.AddDays(x.Nights+rentalInfo.PreparationTimeInDays) > model.Start.Date)
                                           || (x.Start < model.Start.AddDays(model.Nights + rentalInfo.PreparationTimeInDays) && x.Start.AddDays(x.Nights + rentalInfo.PreparationTimeInDays) >= model.Start.AddDays(model.Nights+ rentalInfo.PreparationTimeInDays))
                                           || (x.Start > model.Start && x.Start.AddDays(x.Nights + rentalInfo.PreparationTimeInDays) < model.Start.AddDays(model.Nights+ rentalInfo.PreparationTimeInDays)));
              
                RentalViewModel rental = DAL.Rentals.getRentalById(model.RentalId);
                if (unitStatus.Count>0 )
                    throw new ApplicationException("Not available");
            }


            int insertRows = DAL.Bookings.insert(model);

            //it is not the best
            if (insertRows == 0)
                return null;
            else
            {
                List<BookingViewModel> currentBooking = DAL.Bookings.getBookings();
                ResourceIdViewModel result = new ResourceIdViewModel();
                result.Id = currentBooking.Max(x => x.Id);
                return result;

            }
        }
    }
}
