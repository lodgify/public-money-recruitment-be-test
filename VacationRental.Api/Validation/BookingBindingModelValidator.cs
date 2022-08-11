using System;
using FluentValidation;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Validation
{
    public class BookingBindingModelValidator : AbstractValidator<BookingBindingModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingBindingModelValidator(
            IRentalRepository rentalRepository,
            IBookingRepository bookingRepository
        )
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;

            RuleFor(dto => dto.Nights)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Nights must be positive");

            RuleFor(dto => dto.RentalId)
                .Cascade(CascadeMode.Stop)
                .Must(RentalExists)
                .WithMessage("Rental not Found");

            RuleFor(dto => dto.Start)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Start date is required")
                .NotNull()
                .WithMessage("Start date is required");

            RuleFor(dto => dto)
                .Cascade(CascadeMode.Stop)
                .Must(CheckAvailability)
                .WithMessage("Rental is not available");
        }

        private bool RentalExists(int rentalId) => _rentalRepository.Get(rentalId) != null;

        private bool CheckAvailability(BookingBindingModel model)
        {
            var rental = _rentalRepository.Get(model.RentalId);

            if (rental is null)
                return false;

            if (_bookingRepository.HasRentalBooking(rental.Id, model.Start,
                    model.Start.Date.AddDays(model.Nights + rental.PreparationTimeInDays),
                    rental.PreparationTimeInDays))
                return false;

            return true;
        }
    }
}