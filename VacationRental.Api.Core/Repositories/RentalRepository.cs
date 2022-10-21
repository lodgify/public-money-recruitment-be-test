using System;
using System.Collections.Generic;
using VacationRental.Api.Core.Helpers;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalRepository(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        public RentalViewModel GetRental(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        public ResourceIdViewModel InsertNewRental(RentalBindingModel model)
        {
            var resource = _rentals.CreateResourceIdForRentals();

            if (model.PreparationTimeInDays <= 0)
                throw new ApplicationException("PreparationTimeInDays must be positive");

            _rentals.Add(resource.Id, new RentalViewModel
            {
                Id = resource.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return resource;
        }

        public bool UpdateRental(int rentalId, RentalBindingModel rentalModel)
        {
            if(!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            _rentals[rentalId].Units = rentalModel.Units;
            _rentals[rentalId].PreparationTimeInDays = rentalModel.PreparationTimeInDays;

            return true;
        }
    }
}
