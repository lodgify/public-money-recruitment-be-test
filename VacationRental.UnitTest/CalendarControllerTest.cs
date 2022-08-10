using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.UnitTest
{
    public class CalendarControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task CalendarService_Get_WhenInvalidModelSend_ReturnException()
        {
            var mockData = new Mock<ICalendarService>();

            mockData.Setup(x => x.GetCalendar(It.IsAny<ReserveBindingModel>()))
                .ThrowsAsync(new ApplicationException("Nights must be positive"));

            var controller = new BaseConfiguration().WithCalendarService(mockData.Object).BuildCalendarController();

            var result =
                await Assert.ThrowsAsync<ApplicationException>(() =>
                    controller.Get(new ReserveBindingModel {Nights = -1}));

            result.Should().BeOfType<ApplicationException>().Which.Message.Should().Be("Nights must be positive");
        }

        [Fact]
        public async Task CalendarService_Get_WhenValidModelSend_ReturnOk()
        {
            var mockData = new Mock<ICalendarService>();
            mockData.Setup(x => x.GetCalendar(It.IsAny<ReserveBindingModel>()))
                .ReturnsAsync(new CalendarViewModel
                {
                    RentalId = 1, Dates = new List<CalendarDateViewModel>()
                    {
                        new() {Date = DateTime.Now, Bookings = new List<CalendarBookingViewModel>() {new() {Id = 1}}}
                    }
                });

            var controller = new BaseConfiguration().WithCalendarService(mockData.Object).BuildCalendarController();

            var result = await controller.Get(It.IsAny<ReserveBindingModel>());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<CalendarViewModel>()
                .Which.RentalId.Should().Be(1);
        }
    }
}