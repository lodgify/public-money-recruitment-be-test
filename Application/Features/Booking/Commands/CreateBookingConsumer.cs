using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Exceptions;
using MassTransit;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Booking.Commands;

public class CreateBookingConsumer: IConsumer<CreateBookingRequest>
{
    private readonly IRepository<Domain.Entities.Booking> _bookingRepository;
    private readonly IRepository<Domain.Entities.Rental> _rentalRepository;
    
    public CreateBookingConsumer(IRepository<Domain.Entities.Booking> bookingRepository, IRepository<Domain.Entities.Rental> rentalRepository)
    {
        _bookingRepository = bookingRepository;
        _rentalRepository = rentalRepository;
    }
    
    public async Task Consume(ConsumeContext<CreateBookingRequest> context)
    {
        var temp = await _rentalRepository.GetByIdAsync(context.Message.RentalId);

        if (temp == null)
        {
            await context.RespondAsync<RentalNotFound>(new RentalNotFound());
        }
        else
        {
            var newBooking = new Domain.Entities.Booking
            {
                Nights = context.Message.Nights,
                RentalId = context.Message.RentalId,
                Start = context.Message.Start.Date,
                Unit = context.Message.Unit,
                IsPreparation = false
            };
        
            _bookingRepository.Add(newBooking);
            await _bookingRepository.SaveChangesAsync();
            
            await context.Publish<CreatePreparationTimeRequest>(new CreatePreparationTimeRequest
            {
                Nights = temp.PreparationTimeInDays,
                RentalId = context.Message.RentalId,
                Start = newBooking.LastDay,
                Unit = context.Message.Unit
            });

            await context.RespondAsync(new ResourceIdViewModel
            {
                Id = newBooking.Id
            });
        }
    }
}



