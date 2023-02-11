using AutoMapper;
using System.Linq;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Entities.Rentals;
using VacationRental.Domain.Errors;
using VacationRental.Domain.Messages.Rentals;
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
            var bookings = _bookingRepository.GetBookingByRentalId(command.RentalId).GroupBy(x => x.Start).ToList();

            if(bookings.Count() == 1)
            {
                var numberOfBookings = 0;
                foreach (var booking in bookings[0])
                {
                    if(RentalManager.HasUnitsAnyConflict(booking, command.PreparationTimeInDays, command.Units, numberOfBookings))
                        throw new ConflictException(RentalError.RentalNotAvailable);                    

                    numberOfBookings++;
                }
            }
            else
            {
                for (var i = 0; i < bookings.Count() - 1; i++)
                {
                    var numberOfBookings = 0;

                    foreach (var booking in bookings[i])
                    {
                        if (RentalManager.HasUnitsAnyConflict(booking, command.PreparationTimeInDays, command.Units, numberOfBookings))                        
                            throw new ConflictException(RentalError.RentalNotAvailable);
                        
                        numberOfBookings++;

                        if (RentalManager.HasPreparationDaysAnyConflicts(bookings[i].Key, bookings[i + 1].Key, booking.Nights, bookings[i + 1].Count(), command.Units, command.PreparationTimeInDays))
                            throw new ConflictException(RentalError.RentalNotAvailable);
                    }
                }
            }
            
            var rental = _rentalRepository.GetById(command.RentalId);
            rental.SetPreparationTimeInDays(command.PreparationTimeInDays);
            rental.SetUnits(command.Units);

            var result = _rentalRepository.Update(rental);

            return _mapper.Map<RentalDto>(result);
            }

        }
    }
