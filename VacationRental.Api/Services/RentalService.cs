using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class RentalService:IRentalService
    {
        public void rentalGetValidation(int rentalId, IDictionary<int, RentalViewModel> _rentals)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");
        }
        public ResourceIdViewModel addResourceIdViewModel(RentalBindingModel model, IDictionary<int, RentalViewModel> _rentals)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });
            return key;
        }
    }
}
