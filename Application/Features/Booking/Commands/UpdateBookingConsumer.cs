using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Exceptions;
using MassTransit;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Booking.Commands;

public class UpdateBookingConsumer : IConsumer<UpdateBookingRequest>
{
    private readonly IRepository<Domain.Entities.Booking> _bookingRepository;
    private readonly IRepository<Domain.Entities.Rental> _rentalRepository;

    public UpdateBookingConsumer(IRepository<Domain.Entities.Booking> bookingRepository,
        IRepository<Domain.Entities.Rental> rentalRepository)
    {
        _rentalRepository = rentalRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task Consume(ConsumeContext<UpdateBookingRequest> context)
    {
        var bookingForUpdate = await _bookingRepository.GetByIdAsync(context.Message.Id);

        if (bookingForUpdate == null)
        {
            await context.RespondAsync<BookingNotFound>(new BookingNotFound());
        }
        else
        {
            bookingForUpdate.Nights = context.Message.Nights;
            bookingForUpdate.RentalId = context.Message.RentalId;
            bookingForUpdate.Start = context.Message.Start.Date;
            bookingForUpdate.Unit = context.Message.Unit;
            bookingForUpdate.IsPreparation = context.Message.IsPreparation;
            
            _bookingRepository.Update(bookingForUpdate);
            await _bookingRepository.SaveChangesAsync();
            
            await context.RespondAsync(new ResourceIdViewModel
            {
                Id = bookingForUpdate.Id
            });
        }
    }
}