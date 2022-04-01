using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.Services.ReCalculateBookingsService;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Rentals.Commands.PutRental
{
    public class PutRentalCommandHandler : IRequestHandler<PutRentalCommand, ResourceIdViewModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IValidateRentalModificationService _service;

        public PutRentalCommandHandler(IRentalRepository rentalRepository, IValidateRentalModificationService service,
            IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _service = service;
            _bookingRepository = bookingRepository;
        }

        public async Task<ResourceIdViewModel> Handle(PutRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.Get(request.Id);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var bookings = _bookingRepository.GetByRentalId(rental.Id);

            if (!_service.Validate(rental, bookings))
                throw new ApplicationException("Update not possible due to dates in conflict");

            rental.Units = request.Units;
            rental.PreparationTimeInDays = request.PreparationTimeInDays;

            var key = _rentalRepository.Update(rental);

            return await Task.FromResult(new ResourceIdViewModel() {Id = key});
        }
    }
}
