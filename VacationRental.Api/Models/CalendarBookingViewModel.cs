﻿namespace VacationRental.Api.Models
{
    public class CalendarBookingViewModel
    {
        public CalendarBookingViewModel(int id, int unit)
        {
            Id = id;
            Unit = unit;
        }
        
        public int Id { get; }
        
        public int Unit { get; }
        
        public override int GetHashCode() => Unit;
    }
}
