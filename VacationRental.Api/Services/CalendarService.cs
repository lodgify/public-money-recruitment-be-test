using System;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Api.Storage;

namespace VacationRental.Api.Services;

public class CalendarService : ICalendarService
{
    private readonly IStateManager _state;
    
    public CalendarService(IStateManager state)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }
    
    public CalendarViewModel Get(int rentalId, DateTime start, int nights)
    {
        var rental = _state.Rentals.SingleOrDefault(x => x.Id == rentalId);
        if (rental == null)
        {
            throw new ArgumentException("Rental not found", nameof(rentalId));
        }
        
        var viewModel = CalendarViewModel.Create(rentalId, start, nights);

        var bookings = _state.Bookings.Where(x =>
            x.RentalId == rentalId &&
            x.IsOverlapping(start, nights, rental.PreparationTimeInDays));

        foreach (var booking in bookings)
        {
            for (var i = 0; i < booking.Nights; i++)
            {
                var dateViewModel = viewModel.Dates
                    .SingleOrDefault(x => x.Date == booking.Start.AddDays(i));

                if (dateViewModel == null) continue;
                
                var b = new CalendarBookingViewModel(booking.Id, booking.Unit);
                dateViewModel.Bookings.Add(b);
            }

            var preparationDateViewModel = viewModel.Dates
                .SingleOrDefault(x => x.Date == booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays));
            
            if (preparationDateViewModel == null) continue;
            
            var p = new CalendarPreparationTimeViewModel(booking.Unit);
            preparationDateViewModel.PreparationTimes.Add(p);
        }
        
        return viewModel;
    }
}