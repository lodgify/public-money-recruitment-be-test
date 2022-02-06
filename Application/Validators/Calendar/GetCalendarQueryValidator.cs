using Application.Business.Queries.Calendar;
using Domain.DAL;
using FluentValidation;

namespace Application.Validators.Calendar
{
    public class GetCalendarQueryValidator : AbstractValidator<GetCalendarQuery>
    {
        private readonly IRepository<Domain.DAL.Models.Rental> _repository;
        
        public GetCalendarQueryValidator(IRepository<Domain.DAL.Models.Rental> repository)
        {
            _repository = repository;
            
            RuleFor(c => c.Request.Nights).GreaterThan(0).WithMessage("Nigts must be positive");
            RuleFor(c => c.Request.RentalId).Must(ValidateRentalId).WithMessage("Rental not found");
        }

        private bool ValidateRentalId(int rentalId)
        {
            return _repository.Query.ContainsKey(rentalId);
        }
    }
}