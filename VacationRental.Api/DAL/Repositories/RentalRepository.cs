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

        public int Add(RentalViewModel model)
        {
            model.Id = _dataContext.RentalId;
            _dataContext.Rentals.Add(model.Id, model);

            return model.Id;
        }

        public RentalViewModel Get(int id) => _dataContext.Rentals[id];

        public bool HasValue(int id)
        {
            return _dataContext.Rentals.ContainsKey(id);
        }

        public void Update(int id, RentalBindingModel model)
        {
            var rental = Get(id);

            rental.Units = model.Units;
            rental.PreparationTimeInDays = model.PreparationTimeInDays;

            _dataContext.Rentals[id] = rental;
        }
    }
}
