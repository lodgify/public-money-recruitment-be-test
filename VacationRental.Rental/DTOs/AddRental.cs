using System.ComponentModel.DataAnnotations;
using VacationRental.Rental.Domain.Interfaces;

namespace VacationRental.Rental.DTOs
{
    public class AddRentalRequest
    {
        [Required]
        public int Units { get; set; }

        [Required]
        public int PreparationTimeInDays { get; set; }

    }

    public class AddRentalResponse : IRentalId
    {
        public int Id { get; set; }
    }
}