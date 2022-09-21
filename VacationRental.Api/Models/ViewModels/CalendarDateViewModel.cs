using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models.ViewModels;

public class CalendarDateViewModel
{
    public DateTime Date { get; set; }
    public List<CalendarBookingViewModel> Bookings { get; set; }
}