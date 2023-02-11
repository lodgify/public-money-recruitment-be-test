﻿using FluentValidation;
using System.Data;

namespace VacationRental.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(c => c.Nights)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Nigts must be positive");
        }

    }
}
