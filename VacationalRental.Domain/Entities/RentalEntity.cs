using System.ComponentModel.DataAnnotations.Schema;

namespace VacationalRental.Domain.Entities
{
    public class RentalEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Units { get; set; }

        public int PreprationTimeInDays { get; set; }

        //Only works for database provider not in memory mode
        public byte[] RowVersion { get; set; }
    }
}
