using Application.Business.Queries.Booking;
using Domain.DAL;
using FluentValidation;

namespace Application.Validators.Booking
{
    public class GetBookingQueryValidator : AbstractValidator<GetBookingQuery>
    {
        private readonly IRepository<Domain.DAL.Models.Booking> _repository;

        public GetBookingQueryValidator(IRepository<Domain.DAL.Models.Booking> repository)
        {
            _repository = repository;

            RuleFor(b => b.BookingId).Must(ValidateBookingId).WithMessage("Booking not found");
        }

        private bool ValidateBookingId(int bookingId)
        {
            return _repository.Query.ContainsKey(bookingId);
        }
    }
}