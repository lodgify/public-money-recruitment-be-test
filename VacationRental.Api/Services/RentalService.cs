using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Data;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Services
{
    public partial class RentalService : IRentalService
    {
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IDictionary<int, Rental> _rentals;

        public RentalService(
            IDictionary<int, Booking> bookings,
            IDictionary<int, Rental> rentals)
        {
            _bookings = bookings;
            _rentals = rentals;
        }

        public ResourceIdViewModel Create(RentalBindingModel model)
        {
            if (model == null)
                throw new ApplicationException("The supplied rental model is not appropriate!");

            if (model.Units <= 0)
                throw new ApplicationException("Units must be greater than zero!");

            if (model.PreparationTimeInDays <= 0)
                throw new ApplicationException("PreparationTimeInDays must be greater than zero!");

            int nextRentalId = _rentals.Keys.Count + 1;

            Rental rental = new Rental()
            {
                Id = nextRentalId,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            _rentals.Add(nextRentalId, rental);

            return new ResourceIdViewModel { Id = rental.Id };
        }

        public RentalViewModel Get(int rentalId)
        {
            if (rentalId <= 0)
                throw new ApplicationException("Rental Id must be greater than zero!");

            Rental rental = _rentals.Values.FirstOrDefault(r => r.Id == rentalId);

            if (rental == null)
                throw new ApplicationException($"Rental not found with rentalId:{rentalId}!");

            return new RentalViewModel()
            {
                Id = rental.Id,
                PreparationTimeInDays = rental.PreparationTimeInDays,
                Units = rental.Units
            };
        }

        public RentalViewModel Update(int rentalId, RentalBindingModel model)
        {
            if (rentalId <= 0)
                throw new ApplicationException("Rental Id must be greater than zero!");

            if (model == null)
                throw new ApplicationException("The supplied rental model is not appropriate!");

            RentalViewModel rental = Get(rentalId);

            if (rental == null)
                throw new ApplicationException($"Rental not found with rentalId:{rentalId}!");

            if (model.Units != default)
            {
                ThrowExceptionIfNewUnitNumberIsNotSufficient(rentalId, model);

                rental.Units = model.Units;
            }

            if (model.PreparationTimeInDays != default)
            {
                if (model.PreparationTimeInDays > rental.PreparationTimeInDays)
                    ThrowExceptionIfNewPreparationTimeCauseOverlap(rentalId, model);

                rental.PreparationTimeInDays = model.PreparationTimeInDays;
            }

            Rental updatedRental = new Rental()
            {
                Id = rentalId,
                PreparationTimeInDays = rental.PreparationTimeInDays,
                Units = rental.Units
            };

            _rentals[rentalId] = updatedRental;

            UpdateBookingUnitNumbersIfNecessary(updatedRental);

            return rental;
        }

        private void ThrowExceptionIfNewPreparationTimeCauseOverlap(int rentalId, RentalBindingModel model)
        {
            IEnumerable<PreparationViewModel> updatedPreparationDates = CalculateUpdatedPreparationDates(rentalId, model.PreparationTimeInDays);

            IEnumerable<Booking> bookings = GetBookingsAfterSpesificDate(rentalId, DateTime.Today);

            foreach (Booking booking in bookings)
            {
                for (int i = 0; i < booking.Nights; i++)
                {
                    bool overlap = updatedPreparationDates.Any(x => x.Date == booking.Start.AddDays(i)
                                                                                   && x.Unit == booking.Unit);

                    if (overlap)
                        throw new ApplicationException("The process can not be done! An overlapping occurred due to reducing the number of units!");
                }
            }
        }

        private void ThrowExceptionIfNewUnitNumberIsNotSufficient(int rentalId, RentalBindingModel model)
        {
            RentalViewModel rental = Get(rentalId);

            // If Units number is decreased
            if (rental.Units > model.Units)
            {
                IEnumerable<Booking> rentalFutureBookings = GetBookingsAfterSpesificDate(rental.Id, DateTime.Today);

                foreach (var booking in rentalFutureBookings)
                {
                    for (int i = 0; i < booking.Nights; i++)
                    {
                        int bookingCount = GetBookingCountByDate(rental, booking.Start.AddDays(i).Date);

                        if (bookingCount > model.Units)
                            throw new ApplicationException("Unit number is not sufficient!");
                    }
                }
            }
        }

        private IEnumerable<PreparationViewModel> CalculateUpdatedPreparationDates(int rentalId, int newPreparationTimeInDays)
        {
            IEnumerable<Booking> rentalFutureBookings = GetBookingsAfterSpesificDate(rentalId, DateTime.Today);

            List<PreparationViewModel> newPreparationDates = new List<PreparationViewModel>();

            foreach (Booking booking in rentalFutureBookings)
            {
                DateTime endDate = booking.Start.AddDays(booking.Nights);

                for (int i = 0; i < newPreparationTimeInDays; i++)
                {
                    PreparationViewModel preparation = new PreparationViewModel()
                    {
                        Unit = booking.Unit,
                        Date = endDate.AddDays(i)
                    };

                    newPreparationDates.Add(preparation);
                }
            }

            return newPreparationDates;
        }

        private void UpdateBookingUnitNumbersIfNecessary(Rental rental)
        {
            IEnumerable<Booking> bookingsToUpdate = _bookings.Values.Where(x => x.RentalId == rental.Id
                                                                                             && x.Start.AddDays(x.Nights) >= DateTime.Today
                                                                                             && x.Unit > rental.Units);

            IEnumerable<int> rentalUnits = GetUnits(rental.Units);

            foreach (var booking in bookingsToUpdate)
            {
                for (int i = 0; i < booking.Nights; i++)
                {
                    IEnumerable<Booking> selectedBookings = GetBookingsByDate(rental.Id, booking.Start.AddDays(i));

                    IEnumerable<int> selectedUnits = selectedBookings?.Select(x => x.Unit);

                    bool anyUnitToUpdate = selectedUnits.Any(x => x > rental.Units);

                    if (!anyUnitToUpdate)
                        continue;

                    int newSelectedUnit = rentalUnits.FirstOrDefault(x => !selectedUnits.Contains(x));

                    if (newSelectedUnit == default)
                        throw new ApplicationException("No available unit could found for this process");

                    _bookings[booking.Id].Unit = newSelectedUnit;
                }
            }
        }

        private IEnumerable<Booking> GetBookingsByDate(int rentalId, DateTime date)
        {
            return _bookings.Values.Where(x => x.RentalId == rentalId
                                                            && (x.Start <= date.Date
                                                            && x.Start.AddDays(x.Nights) > date.Date));
        }

        private int GetBookingCountByDate(RentalViewModel rental, DateTime date)
        { 
            return _bookings.Values.Count(x => x.RentalId == rental.Id
                                                            && (x.Start.Date <= date.Date
                                                            && x.Start.AddDays(x.Nights + rental.PreparationTimeInDays).Date >= date.Date));
        }

        private IEnumerable<Booking> GetBookingsAfterSpesificDate(int rentalId, DateTime date)
        {
            return _bookings.Values.Where(x => x.RentalId == rentalId
                                                            && x.Start.AddDays(x.Nights) >= date);
        }

        private List<int> GetUnits(int unitCount)
        {
            List<int> units = new List<int>();

            if (unitCount <= 0)
                return units;

            for (int i = 1; i <= unitCount; i++)
            {
                units.Add(i);
            }

            return units;
        }
    }
}
