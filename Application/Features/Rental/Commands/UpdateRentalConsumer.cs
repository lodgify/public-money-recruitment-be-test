using System.Data.SqlTypes;
using Application.Features.Booking.Commands;
using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Rental.Requests;
using MassTransit;
using MassTransit.Initializers;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Rental.Commands;

public class UpdateRentalConsumer : IConsumer<UpdateRentalRequest>
{
    private readonly IRepository<Domain.Entities.Rental> _rentalRepository;
    private readonly IRepository<Domain.Entities.Booking> _repositoryBooking;


    public UpdateRentalConsumer(IRepository<Domain.Entities.Rental> rentalRepository,
        IRepository<Domain.Entities.Booking> _repositoryBooking)
    {
        _rentalRepository = rentalRepository;
        this._repositoryBooking = _repositoryBooking;
    }

    public async Task Consume(ConsumeContext<UpdateRentalRequest> context)
    {
        var rental = await _rentalRepository.GetByIdAsync(context.Message.Id);

        rental.Units = context.Message.Units;
        rental.PreparationTimeInDays = context.Message.PreparationTimeInDays;

        _rentalRepository.Update(rental);
        await _rentalRepository.SaveChangesAsync();


        var updatePreparationBooking = await _repositoryBooking
            .FindByConditionAsync(x => x.RentalId == rental.Id && x.IsPreparation == true)
            .Select(j => new UpdateBookingRequest
            {
                Id = j.Id,
                Nights = context.Message.PreparationTimeInDays,
                Start = j.Start,
                Unit = j.Unit,
                IsPreparation = j.IsPreparation,
                RentalId = j.RentalId
            });

        await context.Publish<UpdateBookingRequest>(updatePreparationBooking);

        await context.RespondAsync<ResourceIdViewModel>(new ResourceIdViewModel
        {
            Id = rental.Id
        });
    }
}