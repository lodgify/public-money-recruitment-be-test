using Models.ViewModels;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Operations.BookingOperations;

public sealed class BookingCreateOperation : IBookingCreateOperation
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;

    public BookingCreateOperation(IBookingRepository bokingRepository, IRentalRepository rentalRepository)
    {
        _bookingRepository = bokingRepository;
        _rentalRepository = rentalRepository;
    }

    public ResourceIdViewModel ExecuteAsync(BookingBindingViewModel model)
    {
        if (model.Nights <= 0)
            throw new ApplicationException("Nigts must be positive");

        return DoExecute(model);
    }

    private ResourceIdViewModel DoExecute(BookingBindingViewModel model)
    {
        if (!_bookingRepository.IsExists(model.RentalId))
            throw new ApplicationException("Rental not found");

        var bookings = _bookingRepository.GetAll();

        for (var i = 0; i < model.Nights; i++)
        {
            var count = 0;
            foreach (var booking in bookings)
            {
                if (booking.RentalId == model.RentalId
                    && (booking.Start <= model.Start.Date 
                        && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                    || (booking.Start < model.Start.AddDays(model.Nights) &&
                        booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                    || (booking.Start > model.Start &&
                        booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                {
                    count++;
                }
            }

            if (count >= _rentalRepository.Get(model.RentalId).Units)
                throw new ApplicationException("Not available");
        }


        var key = new ResourceIdViewModel { Id = bookings.Count() + 1 };

        _bookingRepository.Create(key.Id, new BookingViewModel
        {
            Id = key.Id,
            Nights = model.Nights,
            RentalId = model.RentalId,
            Start = model.Start.Date
        });

        return key;
    }
}
