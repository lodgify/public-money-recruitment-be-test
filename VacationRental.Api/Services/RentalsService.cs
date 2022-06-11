using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Services
{
    public class RentalsService : GeneralService<RentalViewModel>, IRentalsService
    {
        public RentalsService(IDictionary<int, RentalViewModel> dictionary) : base(dictionary)
        {
        }

        public virtual ResourceIdViewModel AddRental(RentalBindingModel rental)
        {
            var key = new ResourceIdViewModel { Id = GenerateId() };

            Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays,
            });

            return key;
        }
    }
}
