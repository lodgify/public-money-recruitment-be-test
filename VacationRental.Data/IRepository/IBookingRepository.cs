using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Models;

namespace VacationRental.Data.IRepository
{
    public interface IBookingRepository : IBaseRepository<BookingViewModel>
    {
        IReadOnlyCollection<BookingViewModel> GetByRentalId(int rentalId);
    }
}
