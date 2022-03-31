using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Api.Models;
using VacationRental.Application.Bookings.Queries.GetBooking;

namespace VacationRental.Application.Bookings.Commands.PostBooking
{
    public class PostBookingCommandHandler : IRequestHandler<PostBookingCommand, ResourceIdViewModel>
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public PostBookingCommandHandler(IDictionary<int, BookingViewModel> bookings, IDictionary<int, RentalViewModel> rentals)
        {
            _bookings = bookings;
            _rentals = rentals;
        }

        public async Task<ResourceIdViewModel> Handle(PostBookingCommand command, CancellationToken cancellationToken)
        {
            var model = command.Model;

            if (!_rentals.ContainsKey(model.RentalId))
                return null;
            
            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }
                
                if (count >= _rentals[model.RentalId].Units)
                    throw new ApplicationException("Not available");
            }
            
            
            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };
            
            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });
            
            return await Task.FromResult(key);
        }
    }
}