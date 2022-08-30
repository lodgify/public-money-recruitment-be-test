using Application.Models;
using Application.Models.Rental.Requests;
using MassTransit;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Rental.Commands;

public class CreateRentalConsumer : IConsumer<CreateRentalRequest>
{
    private readonly IRepository<Domain.Entities.Rental> _rentalRepository;

    public CreateRentalConsumer(IRepository<Domain.Entities.Rental> rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task Consume(ConsumeContext<CreateRentalRequest> context)
    {
        var newRental = new Domain.Entities.Rental
        {
            Units = context.Message.Units,
            PreparationTimeInDays = context.Message.PreparationTimeInDays
        };

        _rentalRepository.Add(newRental);
        await _rentalRepository.SaveChangesAsync();
        
        await context.RespondAsync<ResourceIdViewModel>(new ResourceIdViewModel
        {
            Id = newRental.Id
        });
    }
}