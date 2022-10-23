using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Infrastructure.Commons;
using VacationRental.Api.Infrastructure.Contracts;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        
        public RentalRepository(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        public async Task<IDictionary<int, RentalViewModel>> GetAllAsync()
            => await Task.FromResult(_rentals);

        public async Task<RentalViewModel> GetByIdAsync(int id)
        {
            try
            {
                return await Task.FromResult(_rentals[id]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResourceIdViewModel> AddAsync(RentalViewModel entityViewModel)
        {
            var resource = _rentals.CreateResourceId();
            entityViewModel.Id = resource.Id;
            _rentals.Add(resource.Id, entityViewModel);
            return await Task.FromResult(resource);
        }

        public async Task<ResourceIdViewModel> UpdateAsync(RentalViewModel entityViewModel)
        {
            _rentals[entityViewModel.Id] = entityViewModel;
            return await Task.FromResult(_rentals[entityViewModel.Id].GetResourceId());
        }

        #region Unused
        public Task<ResourceIdViewModel> DeleteAsync(int rentalId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
