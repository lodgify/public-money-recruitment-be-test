using System.Linq;
using Application.Business.Commands;
using Application.Business.Commands.Booking;
using Application.Models.Booking.Requests;
using Domain.DAL;
using Domain.DAL.Models;
using FluentValidation;

namespace Application.Validators.Booking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        private readonly IRepository<Domain.DAL.Models.Rental> _rentalRepository;
        private readonly IRepository<Domain.DAL.Models.Booking> _bookingRepository;
        
        
        public CreateBookingCommandValidator(IRepository<Domain.DAL.Models.Rental> rentalRepository, IRepository<Domain.DAL.Models.Booking> bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;

            RuleFor(c => c.Request.Nights).GreaterThan(0).WithMessage("Nigts must be positive");
            RuleFor(c => c.Request.RentalId).Must(ValidateRentalId).WithMessage("Rental not found");
            RuleFor(c => c.Request).Must(IsAvailable).WithMessage("Not available");
        }

        private bool ValidateRentalId(int rentalId)
        {
            return _rentalRepository.Query.ContainsKey(rentalId);
        }
        
        private bool IsAvailable(CreateBookingRequest model)
        {
            return !_bookingRepository.Query.Values.Any(booking => 
                booking.RentalId == model.RentalId && booking.Unit == model.Unit 
                                                   && ((booking.Start <= model.Start.Date && booking.LastDay > model.Start.Date) 
                                                       || (booking.Start < model.Start.AddDays(model.Nights) && booking.LastDay >= model.Start.AddDays(model.Nights))
                                                       || (booking.Start > model.Start && booking.LastDay < model.Start.AddDays(model.Nights))));
            
        }
    }
}