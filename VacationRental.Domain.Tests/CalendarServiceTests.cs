using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Business;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Models;
using Xunit;

namespace VacationRental.Domain.Tests
{
    public class CalendarServiceTests
    {
        [Fact]
        public async Task GetRentalCalendarByNights_TwoBookings()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            int rentalId = 1;
            int preparationTimeInDays = 2;
            int nights = 16;
            DateTime start = new DateTime(2021, 04, 07);

            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    Nights = 3,
                    RentalId = rentalId,
                    Start = new DateTime(2021, 04, 07),
                    Unit = 1
                },
                new BookingEntity
                {
                    Id = 2,
                    Nights = 5,
                    RentalId = rentalId,
                    Start = new DateTime(2021, 04, 10),
                    Unit = 2
                }
            };

            var expected = new CalendarModel
            {
                Dates = new List<CalendarDateModel>
                {
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 1,
                                Unit = 1
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = start
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 1,
                                Unit = 1
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 08)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 1,
                                Unit = 1
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 09)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 2,
                                Unit = 2
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>
                        {
                            new PreparationTimesModel
                            {
                                Unit = 1
                            }
                        },
                        Date = new DateTime(2021, 04, 10)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 2,
                                Unit = 2
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>
                        {
                            new PreparationTimesModel
                            {
                                Unit = 1
                            }
                        },
                        Date = new DateTime(2021, 04, 11)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 2,
                                Unit = 2
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 12)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 2,
                                Unit = 2
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 13)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>
                        {
                            new CalendarBookingModel
                            {
                                Id = 2,
                                Unit = 2
                            }
                        },
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 14)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>
                        {
                            new PreparationTimesModel
                            {
                                Unit = 2
                            }
                        },
                        Date = new DateTime(2021, 04, 15)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>
                        {
                            new PreparationTimesModel
                            {
                                Unit = 2
                            }
                        },
                        Date = new DateTime(2021, 04, 16)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 17)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 18)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 19)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 20)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 21)
                    },
                    new CalendarDateModel
                    {
                        Bookings = new List<CalendarBookingModel>(),
                        PreparationTimes = new List<PreparationTimesModel>(),
                        Date = new DateTime(2021, 04, 22)
                    }
                },
                RentalId = rentalId
            };

            rentalRepositoryMock.Setup(a => a.GetRentalPreparationTimeInDays(rentalId)).Returns(Task.FromResult(preparationTimeInDays));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));

            var calendarService = new CalendarService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var result = await calendarService.GetRentalCalendarByNights(rentalId, start, nights);

            for (int dateIteration = 0; dateIteration < expected.Dates.Count; dateIteration++)
            {
                Assert.Equal(expected.Dates[dateIteration].Date, result.Dates[dateIteration].Date);

                for (int bookingIteration = 0; bookingIteration < expected.Dates[dateIteration].Bookings.Count; bookingIteration++)
                {
                    Assert.Equal(expected.Dates[dateIteration].Bookings[bookingIteration].Id, result.Dates[dateIteration].Bookings[bookingIteration].Id);
                    Assert.Equal(expected.Dates[dateIteration].Bookings[bookingIteration].Unit, result.Dates[dateIteration].Bookings[bookingIteration].Unit);
                }

                for (int preprationTimesIteration = 0; preprationTimesIteration < expected.Dates[dateIteration].PreparationTimes.Count; preprationTimesIteration++)
                {
                    Assert.Equal(expected.Dates[dateIteration].PreparationTimes[preprationTimesIteration].Unit, result.Dates[dateIteration].PreparationTimes[preprationTimesIteration].Unit);
                }
            }
        }

        [Fact]
        public async Task GetRentalCalendarByNights_InvalidOperationException_RentalIdZero()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var calendarService = new CalendarService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(async () => await calendarService.GetRentalCalendarByNights(0, DateTime.MinValue, 0));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public async Task GetRentalCalendarByNights_InvalidOperationException_StartMinDate()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var calendarService = new CalendarService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(async () => await calendarService.GetRentalCalendarByNights(1, DateTime.MinValue, 0));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public async Task GetRentalCalendarByNights_InvalidOperationException_NightZero()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var calendarService = new CalendarService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(async () => await calendarService.GetRentalCalendarByNights(1, DateTime.Now.Date, 0));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }
}
