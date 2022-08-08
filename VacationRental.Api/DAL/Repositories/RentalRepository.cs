using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.DAL.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDataContext _dataContext;

        public RentalRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(int key, RentalViewModel model)
        {
            _dataContext.Rentals.Add(key, model);
        }

        public RentalViewModel Get(int id) => _dataContext.Rentals[id];

        public bool HasValue(int id)
        {
            return _dataContext.Rentals.ContainsKey(id);
        }

        public void Update(int id, RentalViewModel model)
        {
            var rental = Get(id);

            rental.Units = model.Units;
            rental.PreparationTimeInDays = model.PreparationTimeInDays;

            _dataContext.Rentals[id] = rental;
        }

        public int Count { get => _dataContext.Rentals.Keys.Count; }
    }
}
