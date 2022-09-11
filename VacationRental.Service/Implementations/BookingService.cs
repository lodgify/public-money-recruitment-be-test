using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Common.Models;
using VacationRental.Data.Entities;
using VacationRental.Repository.Interfaces;
using VacationRental.Service.Interfaces;

namespace VacationRental.Service.Implementations
{
    public class BookingService : BaseService<IBaseRepository<Booking>, BookingViewModel, Booking>, IBookingService
    {

        public BookingService(IMapper mapper, IBookingRepository repository) : base(mapper, repository)
        {
        }

        public BookingViewModel SaveBooking(RentalViewModel rental, BookingViewModel model)
        {
            var item = new BookingViewModel
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            };
            var bookings = GetAll();
            if (!CheckAvailability(item, bookings, rental))
            {
                throw new ApplicationException("Not available");
            }

            return AddOrUpdate(item);
        }

        public bool CheckAvailability(BookingViewModel model, IEnumerable<BookingViewModel> bookings, RentalViewModel rental)
        {
            for (var i = 0; i < model.Nights; i++)
            {
                var count = bookings.Count(booking => booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) < model.Start.AddDays(model.Nights)));
               
                if (count >= rental.Units)
                    return false;
            }
            return true;
        }

        public bool CheckAvailability(IEnumerable<BookingViewModel> bookings, RentalViewModel rental)
        {
            foreach (var booking in bookings.ToList())
            {
                if (CheckAvailability(booking, bookings, rental))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
