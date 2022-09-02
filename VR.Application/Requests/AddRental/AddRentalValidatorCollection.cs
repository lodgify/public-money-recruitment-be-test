using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VR.Application.Requests.AddRental
{
    public class AddRentalValidatorCollection : AbstractValidator<AddRentalRequest>
    {
        public AddRentalValidatorCollection()
        {
            RuleFor(r => r.Units)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Units must be greater then 0");
            RuleFor(r => r.PreparationTimeInDays)
                .GreaterThan(-1)
                .WithMessage("PreparationTimeInDays must be positive");
        }
    }
}
