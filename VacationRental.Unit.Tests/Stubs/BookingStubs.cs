using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.Domain.Models;

namespace VacationRental.Unit.Tests.Stubs
{
    public class BookingStubs
    {
        public static List<Booking> GetRentalBookingsWithThirdUnitAvailable()
        {
            return new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 2,
                    Unit = 1
                },
                new Booking()
                {
                    Id = 2,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 4,
                    Unit = 2
                },
                new Booking()
                {
                    Id = 3,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 3,
                    Unit = 4
                }
            };
        }

        public static List<Booking> GetRentalBookingsWithoutAvailableUnit()
        {
            return new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 2,
                    Unit = 1
                },
                new Booking()
                {
                    Id = 2,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 4,
                    Unit = 2
                },
                new Booking()
                {
                    Id = 3,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 3,
                    Unit = 3
                },
                new Booking()
                {
                    Id = 4,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 1,
                    Unit = 4
                }
            };
        }

        public static List<Booking> GetRentalPossibleCrossBookedUnits()
        {
            return new List<Booking>()
            {
                new Booking()
                {
                    Id = 2,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 4),
                    Nights = 2,
                    Unit = 2
                },
                new Booking()
                {
                    Id = 3,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 7),
                    Nights = 5,
                    Unit = 3
                },
                new Booking()
                {
                    Id = 4,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 7),
                    Nights = 10,
                    Unit = 4
                },
                new Booking()
                {
                    Id = 5,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 11),
                    Nights = 1,
                    Unit = 5
                },
                new Booking()
                {
                    Id = 6,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 11),
                    Nights = 7,
                    Unit = 6
                },
                new Booking()
                {
                    Id = 7,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 16),
                    Nights = 2,
                    Unit = 7
                },
                new Booking()
                {
                    Id = 8,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 5),
                    Nights = 5,
                    Unit = 8
                },
                new Booking()
                {
                    Id = 9,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 10),
                    Nights = 5,
                    Unit = 9
                },
                new Booking()
                {
                    Id = 10,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 15),
                    Nights = 3,
                    Unit = 10
                },
            };
        }

        public static List<Booking> GetRentalCrossBookedUnits()
        {
            return new List<Booking>()
            {
                new Booking()
                {
                    Id = 3,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 7),
                    Nights = 5,
                    Unit = 3
                },
                new Booking()
                {
                    Id = 4,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 7),
                    Nights = 10,
                    Unit = 4
                },
                new Booking()
                {
                    Id = 5,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 11),
                    Nights = 1,
                    Unit = 5
                },
                new Booking()
                {
                    Id = 6,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 11),
                    Nights = 7,
                    Unit = 6
                },
                new Booking()
                {
                    Id = 7,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 16),
                    Nights = 2,
                    Unit = 7
                },
                new Booking()
                {
                    Id = 8,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 5),
                    Nights = 5,
                    Unit = 8
                },
                new Booking()
                {
                    Id = 9,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 10),
                    Nights = 5,
                    Unit = 9
                },
                new Booking()
                {
                    Id = 10,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 15),
                    Nights = 3,
                    Unit = 10
                },
            };
        }

        public static List<Booking> GetRentalConflictBookings()
        {
            return new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 5),
                    Nights = 2,
                    Unit = 1
                },
                new Booking()
                {
                    Id = 2,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 4,
                    Unit = 2
                },
                new Booking()
                {
                    Id = 3,
                    RentalId = 1,
                    Start = new DateTime(2022, 9, 1),
                    Nights = 2,
                    Unit = 1
                }
            };
        }
    }
}
