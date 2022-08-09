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

            RuleFor(dto=>dto.Start)
                .NotEmpty().WithMessage("Start date is required")
                .NotNull().WithMessage("Start date is required");

            RuleFor(dto => dto).Must(CheckAvailability).WithMessage("Rental is not available");
        }

        private bool RentalExists(int rentalId) => _rentalRepository.Get(rentalId) != null;

        private bool CheckAvailability(BookingBindingModel model)
        {
            int count = 0;
            for (int i = 0; i < model.Nights; i++)
            {
                if (_bookingRepository.HasRentalAvailable(model.RentalId, model.Start, i))
                    count++;
            }

            var rental = _rentalRepository.Get(model.RentalId);

            if (count >= rental.Units)
                return false;

            return true;
        }

    }
}
