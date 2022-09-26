using FluentValidation;
using VacationRental.BusinessLogic.Services.Models;

namespace VacationRental.BusinessLogic.Services.Validators
{
    public class GetCalendarValidator : AbstractValidator<GetCalendarServiceModel>
    {
        public GetCalendarValidator()
        {
            RuleFor(x => x.Nights).GreaterThan(0);
        }
    }
}
