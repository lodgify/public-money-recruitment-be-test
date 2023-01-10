using AutoMapper;
using System.Linq;
using System.Runtime.CompilerServices;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Entities.Bookings;
using VacationRental.Domain.Errors;
using VacationRental.Domain.Messages.Rentals;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Features.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommandHandler : ICommandHandler<UpdateRentalCommand, RentalDto>
    {
        private readonly IRepository<Rental> _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public UpdateRentalCommandHandler(IRepository<Rental> rentalRepository, IBookingRepository bookingRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public RentalDto Handle(UpdateRentalCommand command)
        {
            var bookings = _bookingRepository.GetBookingByRentalId(command.RentalId);

            var numberOfBookings = 0;

            for(var i = 0; i < bookings.Count()-1; i++)
            {
                if (bookings[i].ShouldAddNewBookingToRental(bookings[i].Start, bookings[i].Nights, command.PreparationTimeInDays))
                    numberOfBookings++;

                if (numberOfBookings >= command.Units)
                    throw new ConflictException(RentalError.RentalNotAvailable);

                var endDateTimeBooking = bookings[i].Start.AddDays(bookings[i].Nights + command.PreparationTimeInDays);
                
                if (endDateTimeBooking > bookings[i+1].Start)
                    throw new ConflictException(RentalError.RentalNotAvailable);
            }

            var rental = _rentalRepository.GetById(command.RentalId);
            rental.SetPreparationTimeInDays(command.PreparationTimeInDays);
            rental.SetUnits(command.Units);

            var result = _rentalRepository.Update(rental);

            return _mapper.Map<RentalDto>(result);

        }
    }
}
