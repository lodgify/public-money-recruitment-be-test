using System;
using System.Collections.Generic;
using VacationRental.Domain.Rentals;

namespace VacationRental.Infrastructure.Persistence
{
    public class InMemoryRentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, RentalModel> _rentals = new Dictionary<int, RentalModel>();

        public int Add(RentalModel model)
        {
            model.Id = _rentals.Count + 1;

            _rentals.Add(model.Id, model);

            return model.Id;
        }

        public int Update(RentalModel model)
        {
            if (!_rentals.ContainsKey(model.Id))
                throw new ApplicationException("Rental does not exist");
            _rentals[model.Id] = model;

            return model.Id;
        }

        public RentalModel Get(int id)
        {
            if (!_rentals.ContainsKey(id))
                return null;

            return _rentals[id];
        }
    }
}
