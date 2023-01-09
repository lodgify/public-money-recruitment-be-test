using AutoMapper;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Domain.Messages.Rentals;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Features.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommandHandler : ICommandHandler<UpdateRentalCommand, RentalDto>
    {
        private readonly IRepository<Rental> _rentalRepository;
        private readonly IMapper _mapper;

        public UpdateRentalCommandHandler(IRepository<Rental> rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public RentalDto Handle(UpdateRentalCommand command)
        {
            var rental = _rentalRepository.GetById(command.Id);
            rental.SetPreparationTimeInDays(command.PreparationTimeInDays);
            rental.SetUnits(command.Units);

            var result = _rentalRepository.Update(rental);

            return _mapper.Map<RentalDto>(result);

        }
    }
}
