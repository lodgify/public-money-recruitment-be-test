using System;
using FakeItEasy;
using NUnit.Framework;
using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Services;

namespace VacationRental.Api.Tests.Units
{
    public class BaseServiceTests
    {
        protected IBookingRepository BookingRepository;
        protected IRentalRepository RentalRepository;
        protected IBookingService BookingService;
        protected IRentalService RentalService;
        protected ICalendarService CalendarService;

        [SetUp]
        public void Setup()
        {
            BookingRepository = A.Fake<IBookingRepository>();
            RentalRepository = A.Fake<IRentalRepository>();
            BookingService = new BookingService(RentalRepository, BookingRepository);
            RentalService = new RentalService(RentalRepository, BookingRepository);
            CalendarService = new CalendarService(RentalRepository, BookingRepository);
        }

        protected void RentalNotFoundExceptionChecks(ApplicationException ex)
        {
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Rental not found"));
        }
    }
}
