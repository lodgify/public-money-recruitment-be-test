﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationalRental.Domain.Entities
{
    public class BookingEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }

        public int Unit { get; set; }

        //Only works for database provider not in memory mode
        public byte[] RowVersion { get; set; }
    }
}
