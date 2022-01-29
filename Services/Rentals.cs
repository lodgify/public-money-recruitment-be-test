using System;
using VacationRental.Models.Rental;

namespace VacationRental.Services
{
    public class Rentals
    {
        public static RentalViewModel getRentalById(int rentalId)
        {
            return DAL.Rentals.getRentalById(rentalId);
        }
    }
}
