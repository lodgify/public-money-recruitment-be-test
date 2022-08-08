using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalService(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }
        public ResourceIdViewModel Create(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays,
            });

            return key;
        }

        public RentalViewModel Get(int id)
        {
            if (!_rentals.ContainsKey(id))
                throw new ApplicationException("Rental not found");

            return _rentals[id];
        }

        public void Update(int id, RentalBindingModel model)
        {
            if (!_rentals.ContainsKey(id))
                throw new ApplicationException("Rental not found");

            var rental = _rentals[id];

            // TODO check is crossing will appear if we change it

            rental.Units = model.Units;
            rental.PreparationTimeInDays = model.PreparationTimeInDays;
        }
    }
}
