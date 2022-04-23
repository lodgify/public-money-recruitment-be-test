﻿using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        public int RentalId { get; set; }

        public int Unit { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
