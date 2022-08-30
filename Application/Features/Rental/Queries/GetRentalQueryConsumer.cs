using Application.Models.Exceptions;
using Application.Models.Rental.Requests;
using Application.Models.Rental.Responses;
using MassTransit;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Rental.Queries;

public class GetRentalQueryConsumer : IConsumer<CheckRental>
{
    private readonly IRepository<Domain.Entities.Rental> _rentalRepository;
    
    public GetRentalQueryConsumer(IRepository<Domain.Entities.Rental> rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }
    
    public async Task Consume(ConsumeContext<CheckRental> context)
    {
        var rental = await _rentalRepository.GetByIdAsync(context.Message.Id);

        if (rental == null)
            await context.RespondAsync(new RentalNotFound());
        
        await context.RespondAsync<RentalResponse>(new 
        {
            rental.Units,
            rental.PreparationTimeInDays
        });
    }
}