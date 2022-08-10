using System;
using System.Linq;
using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public RentalService(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }
        public int Create(RentalBindingModel model)
        {
            var rental = new RentalViewModel
            {
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays,
            };

            return _rentalRepository.Add(rental);
        }

        public RentalViewModel Get(int id)
        {
            if (!_rentalRepository.HasValue(id))
                throw new ApplicationException("Rental not found");

            return _rentalRepository.Get(id);
        }

        public void Update(int id, RentalBindingModel model)
        {
            if (!_rentalRepository.HasValue(id))
                throw new ApplicationException("Rental not found");

            if (HasBookingCrossConflicts(id, model))
                throw new ApplicationException("Cannot update because of booking conflicts");

            _rentalRepository.Update(id, model);
        }

        private bool HasBookingCrossConflicts(int id, RentalBindingModel model)
        {
            var bookings = _bookingRepository.GetBookingsByRentalId(id).ToList();
            if (!bookings.Any())
                return false;

            var startDate = bookings.Min(p => p.Start);
            var endDate = bookings.Max(p => p.End);

            for (var i = 0; startDate.AddDays(i) <= endDate; i++)
            {
                var date = startDate.AddDays(i);

                var bookingCount = bookings.Count(
                    p => p.Start <= date &&
                    p.End.AddDays(model.PreparationTimeInDays) >= date);

                if (bookingCount > model.Units)
                    return true;
            }

            return false;
        }
    }
}
