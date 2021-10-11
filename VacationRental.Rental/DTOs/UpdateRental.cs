using System.ComponentModel.DataAnnotations;
using VacationRental.Rental.Domain.Interfaces;

namespace VacationRental.Rental.DTOs
{
    public class UpdateRentalRequest : IRental
    {
        [Required]
        public int Units { get; set; }

        [Required]
        public int PreparationTimeInDays { get; set; }

        public int Id { get; set; }


    }

    public class UpdateRentalResponse : IRental
    {
        public int Id { get; set; }
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }

    }
}