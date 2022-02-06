using Application.Business.Queries.Rental;
using Domain.DAL;
using FluentValidation;

namespace Application.Validators.Rental
{
    public class GetRentalQueryValidator : AbstractValidator<GetRentalQuery>
    {
        private readonly IRepository<Domain.DAL.Models.Rental> _repository;
        
        public GetRentalQueryValidator(IRepository<Domain.DAL.Models.Rental> repository)
        {
            _repository = repository;
            
            RuleFor(c => c.RentalId).Must(ValidateRentalId).WithMessage("Rental not found");
        }
        
        private bool ValidateRentalId(int rentalId)
        {
            return _repository.Query.ContainsKey(rentalId);
        }
    }
}