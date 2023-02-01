using System;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Api.Providers;
using VacationRental.Api.Storage;

namespace VacationRental.Api.Services;

public class BookingService : IBookingService
{
    private readonly IIdGenerator _idGenerator;
    private readonly IStateManager _state;
    
    public BookingService(
        IIdGenerator idGenerator,
        IStateManager state)
    {
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }
    
    public BookingViewModel Get(int bookingId)
    {
        try
        {
            return _state.Bookings.Single(x => x.Id == bookingId);            }
        catch
        {
            throw new ArgumentException($"{nameof(bookingId)} doesn't exist.");
        }
    }

    public ResourceIdViewModel Add(BookingBindingModel model)
    {
        var rental = _state.Rentals.SingleOrDefault(x => x.Id == model.RentalId);
        if (rental == null)
        {
            throw new ApplicationException("Rental not found");
        }

        for (var unit = 1; unit <= rental.Units; unit++)
        {
            var bookings = _state.Bookings.Where(x =>
                x.RentalId == model.RentalId &&
                x.Unit == unit)
                .ToList();

            var isUnitAvailable = true;
            for (var b = 0; b < bookings.Count && isUnitAvailable; b++)
            {
                var booking = bookings[b];
                if (booking.IsOverlapping(model.Start, model.Nights, rental.PreparationTimeInDays))
                {
                    isUnitAvailable = false;
                }
            }

            if (!isUnitAvailable) continue;
            
            var key = new ResourceIdViewModel { Id = _idGenerator.Generate(_state.Bookings) };

            _state.Bookings.Add(new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = unit
            });
        
            return key;
        }

        throw new ApplicationException("No units available.");
    }
}