using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalService
    {
        void rentalGetValidation(int rentalId, IDictionary<int, RentalViewModel> _rentals);
        ResourceIdViewModel addResourceIdViewModel(RentalBindingModel model, IDictionary<int, RentalViewModel> _rentals);
    }
}
