using VacationRental.Domain.VacationRental.Interfaces.Repositories;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Infra.Repoitory
{
    public class RentalsRepository : IRentalsRepository
    {
        private readonly DatabaseInMemoryContext _context;
        public RentalsRepository(DatabaseInMemoryContext context)
        {
            _context = context;
        }

        public async Task<RentalViewModel?> Get(int rentalId)
        {
            var result = _context?.Rental?.First(x => x.Id == rentalId);
            return result;
        }

        public async Task<List<RentalViewModel>?> Get()
        {
            var result = _context?.Rental?.ToList();
            return result;
        }

        public async Task<int?> GetLastId()
        {
            var result = _context?.Rental?.Select(x => x.Id).LastOrDefault();
            return result;
        }

        public async Task<ResourceIdViewModel> Post(RentalViewModel rentalModel)
        {
            _context.Entry(rentalModel).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.SaveChanges();

            var key = new ResourceIdViewModel { Id = rentalModel.Id };

            return (key);
        }

        public async Task<ResourceIdViewModel> Put(int rentalId, RentalBindingModel rentalModel)
        {
            var rentalTable = _context?.Rental?.First(x => x.Id == rentalId);
            rentalTable.Units = rentalModel.Units;
            rentalTable.PreparationTimeInDays = rentalModel.PreparationTimeInDays;

            _context.Entry(rentalTable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            var key = new ResourceIdViewModel { Id = rentalTable.Id };

            return (key);
        }

    }
}
