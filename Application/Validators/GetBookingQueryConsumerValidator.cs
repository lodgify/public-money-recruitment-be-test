using Application.Models.Booking.Requests;
using Domain.Entities;
using FluentValidation;
using Persistence.Contracts.Interfaces;

namespace Application.Validators;

public class GetBookingQueryConsumerValidator : AbstractValidator<CheckBooking>
{
    private readonly IRepository<Booking> _bookingRepository;

    public GetBookingQueryConsumerValidator(IRepository<Booking> bookingRepository)
    {
        _bookingRepository = bookingRepository;
        RuleFor(r => r.Id).Must(VerifyBookinglExistence);
        RuleFor(r => r.Id).GreaterThanOrEqualTo(1);
    }

    private bool VerifyBookinglExistence(int rentalId)
    {
        var bookingTask =  _bookingRepository.GetByIdAsync(rentalId);
        Task.WaitAll(bookingTask);
        return bookingTask.Result != null;
    }
}