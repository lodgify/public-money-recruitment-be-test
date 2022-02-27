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
        private readonly IDictionary<int, Rental> _rentals;

        public RentalService(IDictionary<int, Rental> rentals)
        {
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
    }
}
