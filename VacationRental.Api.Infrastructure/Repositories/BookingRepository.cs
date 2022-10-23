using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Infrastructure.Commons;
using VacationRental.Api.Infrastructure.Contracts;
using VacationRental.Api.Infrastructure.Models;
using System;

namespace VacationRental.Api.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingRepository(IDictionary<int, BookingViewModel> bookings)
        {
            _bookings = bookings;
        }

        public async Task<IDictionary<int, BookingViewModel>> GetAllAsync()
            => await Task.FromResult(_bookings);

        public async Task<BookingViewModel> GetByIdAsync(int id)
        {
            try
            {
                return await Task.FromResult(_bookings[id]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResourceIdViewModel> AddAsync(BookingViewModel entityViewModel)
        {
            var resource = _bookings.CreateResourceId();
            entityViewModel.Id = resource.Id;
            _bookings.Add(resource.Id, entityViewModel);
            return await Task.FromResult(resource);
        }

        public async Task<IEnumerable<BookingViewModel>> GetAllByRentalIdAsync(int rentalId)
            => await Task.FromResult(_bookings.Values.Where(e => e.RentalId.Equals(rentalId)));

        #region Unused methods
        public Task<ResourceIdViewModel> UpdateAsync(BookingViewModel rentalViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResourceIdViewModel> DeleteAsync(int rentalId)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
