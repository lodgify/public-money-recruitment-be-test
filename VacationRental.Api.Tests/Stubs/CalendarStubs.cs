using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Tests.Stubs
{
    internal class CalendarStubs
    {
        public static CalendarViewModel GenerateCalendar()
        {
            var result = new CalendarViewModel()
            {
                RentalId = 1,
                Dates = new List<CalendarDateViewModel>()
                {
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 10),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new CalendarBookingViewModel()
                            {
                                Id = 1,
                                Unit = 1
                            },
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 11),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new CalendarBookingViewModel()
                            {
                                Id = 1,
                                Unit = 1
                            },
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 12),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new CalendarBookingViewModel()
                            {
                                Id = 1,
                                Unit = 1
                            },
                            new CalendarBookingViewModel()
                            {
                                Id = 2,
                                Unit = 2
                            },
                            new CalendarBookingViewModel()
                            {
                                Id = 3,
                                Unit = 3
                            },
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 13),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new CalendarBookingViewModel()
                            {
                                Id = 1,
                                Unit = 1
                            },
                            new CalendarBookingViewModel()
                            {
                                Id = 2,
                                Unit = 2
                            },
                            new CalendarBookingViewModel()
                            {
                                Id = 3,
                                Unit = 3
                            },
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 14),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new CalendarBookingViewModel()
                            {
                                Id = 1,
                                Unit = 1
                            },
                            new CalendarBookingViewModel()
                            {
                                Id = 2,
                                Unit = 2
                            },
                            new CalendarBookingViewModel()
                            {
                                Id = 3,
                                Unit = 3
                            },
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 15),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new CalendarBookingViewModel()
                            {
                                Id = 3,
                                Unit = 3
                            },
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new CalendarPreparationTimeViewModel()
                            {
                                Unit = 1
                            },
                            new CalendarPreparationTimeViewModel()
                            {
                                Unit = 2
                            }
                        }
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 16),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new CalendarBookingViewModel()
                            {
                                Id = 3,
                                Unit = 3
                            },
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new CalendarPreparationTimeViewModel()
                            {
                                Unit = 1
                            },
                            new CalendarPreparationTimeViewModel()
                            {
                                Unit = 2
                            }
                        }
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 17),
                        Bookings = new List<CalendarBookingViewModel>(),
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new CalendarPreparationTimeViewModel()
                            {
                                Unit = 3
                            }
                        }
                    },
                    new CalendarDateViewModel()
                    {
                        Date = new DateTime(2022, 9, 18),
                        Bookings = new List<CalendarBookingViewModel>(),
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new CalendarPreparationTimeViewModel()
                            {
                                Unit = 3
                            },
                        }
                    }
                }
            };

            return result;
        }
    }
}
