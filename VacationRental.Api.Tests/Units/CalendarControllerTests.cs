using System;
using DeepEqual.Syntax;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using VacationRental.Api.Controllers;
using VacationRental.Api.Services;
using VacationRental.Api.Tests.Stubs;

namespace VacationRental.Api.Tests.Units
{
    public class CalendarControllerTests
    {
        private ICalendarService _calendarService;
        private CalendarController _calendarController;

        [SetUp]
        public void Setup()
        {
            _calendarService = A.Fake<ICalendarService>();
            _calendarController = new CalendarController(_calendarService);
        }

        [Test]
        public void ShouldReturn500StatusCodeIfCalendarIsNotGeneratedOnGet()
        {
            A.CallTo(() => _calendarService.GetCalendar(A<int>._, A<DateTime>._, A<int>._)).Returns(null);

            var result = _calendarController.Get(3, DateTime.Today, 3) as StatusCodeResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public void ShouldReturnCalendarViewModelOnGet()
        {
            var expectedResult = CalendarStubs.GenerateCalendar();
            A.CallTo(() => _calendarService.GetCalendar(A<int>._, A<DateTime>._, A<int>._)).Returns(expectedResult);

            var result = _calendarController.Get(3, DateTime.Today, 3) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            result.Value.ShouldDeepEqual(expectedResult);
        }
    }
}
