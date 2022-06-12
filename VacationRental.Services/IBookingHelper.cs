using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Services
{
    public interface IBookingHelper
    {
        bool CheckVacancy(int rentalId, DateTime start, DateTime end);
        bool CheckCanChange(int rentalId, int units, int preparationTime);
    }
}
