using System;
using System.Linq;
using Application.Business.Commands.Rental;
using Application.Models.Rental.Requests;
using Application.Services.Interfaces;
using Domain.DAL;
using FluentValidation;

namespace Application.Validators.Rental
{
    public class UpdateRentalCommandValidator : AbstractValidator<UpdateRentalCommand>
    {
        private readonly IRepository<Domain.DAL.Models.Rental> _rentalRepository;
        private readonly IRepository<Domain.DAL.Models.Booking> _bookingRepository;
        private readonly ITimeProviderService _timeProviderService;

        public UpdateRentalCommandValidator(IRepository<Domain.DAL.Models.Rental> rentalRepository,
            IRepository<Domain.DAL.Models.Booking> bookingRepository, ITimeProviderService timeProviderService)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
            _timeProviderService = timeProviderService;

            RuleFor(c => c.Request.Id).Must(ValidateRentalId).WithMessage("Rental not found");
            RuleFor(c => c.Request).Must(CanUpdateAmountOfUnits).WithMessage("Units are booked");
            RuleFor(c => c.Request.PreparationTimeInDays).GreaterThan(0);
            RuleFor(c => c.Request).Must(CanUpdatePreparationDates).WithMessage("Unable to update amount of preparation days");
        }

        private bool ValidateRentalId(int rentalId)
        {
            return _rentalRepository.Query.ContainsKey(rentalId);
        }

        private bool CanUpdateAmountOfUnits(UpdateRentalRequest request)
        {
            var now = _timeProviderService.Now();

            var bookings = _bookingRepository.Query.Values.Where(b =>
                b.RentalId == request.Id && b.LastDay >= now);

            return bookings.GroupBy(b => b.Unit).Count() <= request.Units;
        }

        private bool CanUpdatePreparationDates(UpdateRentalRequest request)
        {
            var rental = _rentalRepository.Find(request.Id);
            if (rental.PreparationTimeInDays >= request.PreparationTimeInDays) return true;
            
            var now = _timeProviderService.Now();
            
            var bookings = _bookingRepository.Query.Values.Where(b =>
                b.RentalId == request.Id && b.LastDay >= now && b.IsPreparation).OrderBy(b => b.Start).ToList();

            if (!bookings.Any()) return true;

            var dif = request.PreparationTimeInDays - rental.PreparationTimeInDays;

            return !bookings.Any(b =>
            {
                return _bookingRepository.Query.Values.Any(br =>br.RentalId == b.RentalId && br.Unit ==b.Unit && !br.IsPreparation && br.Start >= b.LastDay && br.Start < b.LastDay.AddDays(dif));
            });  
        }
    }
}