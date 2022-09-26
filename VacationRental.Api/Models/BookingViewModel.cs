﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace VacationRental.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}