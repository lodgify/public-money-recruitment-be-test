using System.Collections.Generic;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Repositories
{
    internal interface IBookingRepository
    {
        Booking Get(int id);
        IReadOnlyCollection<Booking> GetAll(int rentalId);
        int Add(Booking booking);
    }
}
