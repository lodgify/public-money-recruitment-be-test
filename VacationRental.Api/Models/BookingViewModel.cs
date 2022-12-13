﻿using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public DateTime End { get => Start.AddDays(Nights); }
        public int Unit { get; set; }
    }
}
