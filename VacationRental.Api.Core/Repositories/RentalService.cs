using LanguageExt;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Core.Helpers;
using VacationRental.Api.Core.Helpers.Exceptions;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Contracts;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Core.Repositories
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingReponsitory;

        public RentalService(IRentalRepository rentalRepository, IBookingRepository bookingReponsitory)
        {
            _rentalRepository = rentalRepository;
            _bookingReponsitory = bookingReponsitory;
        }

        public async Task<Result<RentalViewModel>> GetRentalByIdAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
                return await Task.FromResult(new Result<RentalViewModel>(new NotFoundException("Rental not found", rentalId)));

            return rental;
        }

        public async Task<Result<ResourceIdViewModel>> InsertNewRentalAsync(RentalBindingModel model)
        {
            if(model.PreparationTimeInDays <= 0)
                return await Task.FromResult(new Result<ResourceIdViewModel>(new NegativeArgumentException("Must be positive", nameof(model.PreparationTimeInDays))));

            return await _rentalRepository.AddAsync(new RentalViewModel
            {
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });
        }

        public async Task<Result<ResourceIdViewModel>> UpdateRentalAsync(int rentalId, RentalBindingModel rentalModel)
        {
            var currentBookings = await _bookingReponsitory.GetAllByRentalIdAsync(rentalId);
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            // Check current booking has overlap
            var hasConflictBookings = CommonHelper.CheckConflictBookings(currentBookings, rental, rentalModel);

            if (hasConflictBookings)
                return await Task.FromResult(new Result<ResourceIdViewModel>(new RentOverlappedException("Rent overlapped", rentalId)));
            if(rental == null)
                return await Task.FromResult(new Result<ResourceIdViewModel>(new NotFoundException("Rental not found", rentalId)));

            rental.Units = rentalModel.Units;
            rental.PreparationTimeInDays = rentalModel.PreparationTimeInDays;

            var updated = await _rentalRepository.UpdateAsync(rental);
            if (updated.Id == 0)
                return await Task.FromResult(new Result<ResourceIdViewModel>(new UpdateFailedException("Update Failed", rentalId)));

            return updated;
        }
    }
}
