using FluentValidation;
using System;

namespace VR.Application.Requests.AddBooking
{
    public class AddBookingValidatorCollection : AbstractValidator<AddBookingRequest>
    {
        public AddBookingValidatorCollection()
        {
            RuleFor(r => r.Nights)
                .GreaterThan(0)
                .WithMessage("Nights must be greater then 0");
            RuleFor(r => r.RentalId)
                .NotEmpty()
                .WithMessage("Should have valid rentalId");
            RuleFor(r => r.Start)
                .GreaterThan(DateTime.Now)
                .WithMessage("Start can't be before now");
        }
    }
}
