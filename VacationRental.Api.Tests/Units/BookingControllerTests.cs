using DeepEqual.Syntax;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using VacationRental.Api.Controllers;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Api.Tests.Stubs;

namespace VacationRental.Api.Tests.Units
{
    public class BookingControllerTests
    {
        private IBookingService _bookingService;
        private BookingsController _bookingsController;

        [SetUp]
        public void Setup()
        {
            _bookingService = A.Fake<IBookingService>();
            _bookingsController = new BookingsController(_bookingService);
        }

        [Test]
        public void ShouldReturnNotFoundIfBookingIsNotExistOnGet()
        {
            A.CallTo(() => _bookingService.Get(A<int>._)).Returns(null);

            var result = _bookingsController.Get(3) as StatusCodeResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public void ShouldReturnBookingViewModelIfBookingIsExistOnGet()
        {
            var expectedResult = BookingStubs.SingleBooking();
            A.CallTo(() => _bookingService.Get(A<int>._)).Returns(expectedResult);

            var result = _bookingsController.Get(3) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            result.Value.ShouldDeepEqual(expectedResult);
        }

        [Test]
        public void ShouldReturn500StatusCodeIfBookingWasNotCreatedOnPost()
        {
            A.CallTo(() => _bookingService.Create(A<BookingBindingModel>._)).Returns(0);

            var result = _bookingsController.Post(BookingStubs.BindingBookingModel()) as StatusCodeResult;


            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public void ShouldReturnIdIfBookingCreateSuccessfullyOnPost()
        {
            var expectedResult = 1;
            A.CallTo(() => _bookingService.Create(A<BookingBindingModel>._)).Returns(expectedResult);

            var result = _bookingsController.Post(BookingStubs.BindingBookingModel()) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            result.Value.ShouldDeepEqual(expectedResult);
        }
    }
}
