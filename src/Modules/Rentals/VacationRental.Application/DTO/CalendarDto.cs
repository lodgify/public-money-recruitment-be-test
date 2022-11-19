﻿using System;
using System.Collections.Generic;

namespace VacationRental.Application.DTO
{
    internal class CalendarDto
    {
        public int RentalId { get; set; }
        public List<CalendarDateDto> Dates { get; set; }
    }

    internal class CalendarDateDto
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingDto> Bookings { get; set; }
    }

    internal class CalendarBookingDto
    {
        public int Id { get; set; }
    }
}
