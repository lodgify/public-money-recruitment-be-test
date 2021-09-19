using FluentValidation;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation.ValidationUnit
{
    public class BokingValidator : AbstractValidator<BookingBindingModel>
    {
        public BokingValidator(IDictionary<int, RentalViewModel> rentals)
        {
            this.RuleFor(model => model.Nights).NotEmpty().NotNull().GreaterThan(0);            
            this.RuleFor(model => model.Start).NotNull().GreaterThan(DateTime.Now);
            this.RuleFor(model => model.Units).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
