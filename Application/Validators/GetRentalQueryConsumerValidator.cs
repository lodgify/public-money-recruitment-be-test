using Application.Models.Rental.Requests;
using Domain.Entities;
using FluentValidation;
using Persistence.Contracts.Interfaces;

namespace Application.Validators;

public class GetRentalQueryConsumerValidator : AbstractValidator<CheckRental>
{
    private readonly IRepository<Rental> _rentalRepository;

    public GetRentalQueryConsumerValidator(IRepository<Rental> rentalRepository)
    {
        _rentalRepository = rentalRepository;
        RuleFor(r => r.Id).GreaterThanOrEqualTo(1);
    }
    
    private bool VerifyRentalExistence(int rentalId)
    {
        var rentalTask =  _rentalRepository.GetByIdAsync(rentalId);
        Task.WaitAll(rentalTask);
        return rentalTask.Result != null;
    }
}