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
            _rentals.Add(resource.Id, new RentalViewModel
            {
                Id = resource.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return resource;
        }
    }
}
