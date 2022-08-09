using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalRepository(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        public int RentalsCount() => _rentals.Keys.Count;
        public RentalViewModel Get(int id) => _rentals.FirstOrDefault(x => x.Key == id).Value;

        public int Create(RentalViewModel model)
        {
            _rentals.Add(model.Id, model);
            
            return model.Id;
        }
    }
}