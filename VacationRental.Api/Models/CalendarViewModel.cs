using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public HashSet<CalendarDateViewModel> Dates { get; set; }

        public static CalendarViewModel Create(int rentalId, DateTime start, int nights)
        {
            var viewModel = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new HashSet<CalendarDateViewModel>(nights + 1)
            };

            for (var night = 0; night < nights; night++)
            {
                viewModel.Dates.Add(new CalendarDateViewModel
                {
                    Date = start.AddDays(night)
                });
            }

            return viewModel;
        }
    }
}
