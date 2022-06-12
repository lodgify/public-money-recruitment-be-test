using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Data.IRepository;
using VacationRental.Data.Store;
using VacationRental.Domain.Models;

namespace VacationRental.Data.Repository
{
    public class RentalRepository : BaseRepository, IRentalRepository
    {
        private IDictionary<int, RentalViewModel> _storeContext;
        public RentalRepository(IDataContext context) : base(context)
        {
            _storeContext = context.Rentals;
        }

        public RentalViewModel Add(RentalViewModel entity)
        {
            int id = _storeContext.Count + 1;
            entity.Id = id;

            _storeContext.Add(id, entity);

            return entity;
        }

        public bool Delete(int id)
        {
            _storeContext.Remove(id);
            return true;
        }

        public IEnumerable<RentalViewModel> GetAll()
        {
            return _storeContext.Values.ToList();
        }

        public RentalViewModel GetById(int id)
        {
            if (!_storeContext.ContainsKey(id))
                throw new ApplicationException("Booking not found");

            return _storeContext[id];
        }

        public RentalViewModel Update(RentalViewModel entity)
        {
            if (!_storeContext.ContainsKey(entity.Id))
                throw new ApplicationException("Rental not found");

            RentalViewModel rental = _storeContext[entity.Id];
            rental.PreparationTimeInDays = entity.PreparationTimeInDays;
            rental.Units = entity.Units;

            return rental;
        }
    }
}
