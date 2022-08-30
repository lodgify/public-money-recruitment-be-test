using Application.Models.Booking.Requests;
using Domain.Entities;
using FluentValidation;
using Persistence.Contracts.Interfaces;

namespace Application.Validators;

public class CreateBookingConsumerValidator : AbstractValidator<CreateBookingRequest>
{
    private readonly IRepository<Rental> _rentalRepository;
    
    public CreateBookingConsumerValidator(IRepository<Rental> rentalRepository)
    {
        _rentalRepository = rentalRepository;
        RuleFor(r => r.RentalId).Must(VerifyRentalExistence);
        RuleFor(r => r.RentalId).NotNull();
        RuleFor(r => r.RentalId).GreaterThanOrEqualTo(1)
            .WithMessage("Rental Identifier should be equal or greater than one");
        RuleFor(r => r.Nights).GreaterThanOrEqualTo(1).WithMessage("Nights should be equal or greater than one");
    }

    private bool VerifyRentalExistence(int rentalId)
    {
        var rentalTask =  _rentalRepository.GetByIdAsync(rentalId);
        Task.WaitAll(rentalTask);
        return rentalTask.Result != null;
    }
}