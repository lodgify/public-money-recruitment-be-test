
using VacationRental.Api.Data.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Data.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDataContext _context;

        public RentalRepository(IDataContext dataContext)
        {
            _context = dataContext;
        }

        public RentalViewModel Get(int id) => _context.Rentals[id];

        public bool HasValue(int id)
        {
            return _context.Rentals.ContainsKey(id);
        }

        public int Insert(RentalViewModel model)
        {
            model.Id = _context.RentalId;
            _context.Rentals.TryAdd(model.Id, model);

            return model.Id;
        }

        public void Update(int id, RentalBindingModel model)
        {
            var rental = Get(id);

            rental.Units = model.Units;
            rental.PreparationTimeInDays = model.PreparationTimeInDays;

            _context.Rentals[id] = rental;
        }
    }
}
