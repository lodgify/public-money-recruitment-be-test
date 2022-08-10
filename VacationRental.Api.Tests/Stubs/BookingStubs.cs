using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Tests.Stubs
{
    public class BookingStubs
    {
        public static List<BookingViewModel> BookingWithCrossDays()
        {
            return new List<BookingViewModel>()
            {
                new BookingViewModel()
                {
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 10),
                    Nights = 5,
                    Unit = 1,
                    Id = 1
                },
                new BookingViewModel()
                {
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 12),
                    Nights = 3,
                    Unit = 2,
                    Id = 2
                },
                new BookingViewModel()
                {
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 12),
                    Nights = 5,
                    Unit = 3,
                    Id = 3
                }
            };
        }

        public static List<BookingViewModel> BookingsWithoutCrossDays()
        {
            return new List<BookingViewModel>()
            {
                new BookingViewModel()
                {
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 10),
                    Nights = 5,
                    Unit = 1,
                    Id = 1
                },
                new BookingViewModel()
                {
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 18),
                    Nights = 3,
                    Unit = 1,
                    Id = 2
                },
                new BookingViewModel()
                {
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 24),
                    Nights = 5,
                    Unit = 1,
                    Id = 3
                }
            };
        }

        public static List<BookingViewModel> EmptyBookingList()
        {
            return new List<BookingViewModel>();
        }
    }
}
