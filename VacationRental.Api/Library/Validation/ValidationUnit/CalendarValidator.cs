using FluentValidation;
using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation.ValidationUnit
{
    public class CalendarValidator : AbstractValidator<CalendarBindingModel>
    {
        public CalendarValidator()
        {
            this.RuleFor(model => model.Nights).NotEmpty().NotNull().GreaterThan(0);
            this.RuleFor(model => model.RentalId).NotEmpty().NotNull().GreaterThan(0);
            this.RuleFor(model => model.Start).GreaterThan(DateTime.Now);
        }
    }
}
