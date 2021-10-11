using System.ComponentModel.DataAnnotations;
using VacationRental.Rental.Domain.Interfaces;

namespace VacationRental.Rental.DTOs
{
    public class GetRentalResponse : IRental
    {
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
        public int Id { get; set; }


    }

    public class GetRentalRequest : IRentalId
    {
        [Required]
        public int Id { get; set; }
    }
}