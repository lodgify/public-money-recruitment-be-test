using Application.Models.Booking.Requests;
using MassTransit;
using Domain.Entities;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Booking.Commands;

public class CreatePreparationTimeConsumer : IConsumer<CreatePreparationTimeRequest>
{
    private readonly IRepository<Domain.Entities.Booking> _bookingRepository;
    private readonly IRepository<Domain.Entities.Rental> _rentalRepository;
    
    public CreatePreparationTimeConsumer(IRepository<Domain.Entities.Booking> bookingRepository, IRepository<Domain.Entities.Rental> rentalRepository)
    {
        _bookingRepository = bookingRepository;
        _rentalRepository = rentalRepository;
    }
    
    public async Task Consume(ConsumeContext<CreatePreparationTimeRequest> context)
    {
        var newBookingPreparation = new Domain.Entities.Booking
        {
            Nights = context.Message.Nights,
            RentalId = context.Message.RentalId,
            Start = context.Message.Start.Date,
            Unit = context.Message.Unit,
            IsPreparation = true
        };
        
        _bookingRepository.Add(newBookingPreparation);
         
        await _bookingRepository.SaveChangesAsync();
    }
}