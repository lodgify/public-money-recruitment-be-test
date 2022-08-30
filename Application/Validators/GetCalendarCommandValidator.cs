using Application.Models.Calendar.Requests;
using Domain.Entities;
using FluentValidation;
using Persistence.Contracts.Interfaces;

namespace Application.Validators;

public class GetCalendarCommandValidator : AbstractValidator<CalendarRequest>
{
    private readonly IRepository<Rental> _rentalRepository;

    public GetCalendarCommandValidator(IRepository<Rental> rentalRepository)
    {
        _rentalRepository = rentalRepository;
        RuleFor(r => r.RentalId).Must(VerifyRentalExistence).WithMessage("RentalId not valid");
        RuleFor(r => r.RentalId).NotNull();
        RuleFor(r => r.RentalId).GreaterThanOrEqualTo(1)
            .WithMessage("Rental Identifier should be equal or greater than one");
        RuleFor(r => r.Nights).GreaterThanOrEqualTo(1).WithMessage("Nights should be equal or greater than one");
    }
    
    private bool VerifyRentalExistence(int rentalId)
    {
        var rentalTask = _rentalRepository.GetByIdAsync(rentalId);
        Task.WaitAll(rentalTask);
        return rentalTask.Result != null;
    }
}