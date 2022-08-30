using Application.Models.Booking.Requests;
using Application.Models.Booking.Responses;
using Application.Models.Exceptions;
using MassTransit;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Booking.Queries;

public class GetBookingQueryConsumer : IConsumer<CheckBooking>
{
    private readonly IRepository<Domain.Entities.Booking> _bookingRepository;
    
    public GetBookingQueryConsumer(IRepository<Domain.Entities.Booking> bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public async Task Consume(ConsumeContext<CheckBooking> context)
    {
        var booking = await _bookingRepository.GetByIdAsync(context.Message.Id);
            
        if (booking == null)
            await context.RespondAsync(new BookingNotFound());
        
        await context.RespondAsync<BookingResponse>(new 
        {
            booking.RentalId,
            booking.Unit,
            booking.Start,
            booking.LastDay,
            booking.Nights,
            booking.IsPreparation
        });
    }
}
