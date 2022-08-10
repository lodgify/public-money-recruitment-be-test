using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.UnitTest
{
    public class BookingsControllerTest : BaseConfiguration
    {
        [Fact]
        public void BookingService_Get_WhenInvalidIdSend_ReturnException()
        {
            var mockData = new Mock<IBookingService>();

            mockData.Setup(x => x.GetBooking(It.IsAny<int>()))
                .Throws(new ApplicationException("Booking not found"));

            var controller = new BaseConfiguration().WithBookingService(mockData.Object).BuildBookingsController();

            Action act = () => controller.Get(It.IsAny<int>());

            act.Should().Throw<ApplicationException>().WithMessage("Booking not found");
        }

        [Fact]
        public void BookingService_Get_WhenValidIdSend_OkResult()
        {
            var mockData = new Mock<IBookingService>();

            mockData.Setup(x => x.GetBooking(It.IsAny<int>())).Returns(new BookingViewModel
                {Id = 1, Nights = 3, RentalId = 1, Start = DateTime.Today});

            var controller = new BaseConfiguration().WithBookingService(mockData.Object).BuildBookingsController();

            var result = controller.Get(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<BookingViewModel>()
                .Which.Id.Should().Be(1);
        }


        [Fact]
        public async Task BookingService_Create_WhenInvalidModelSend_ReturnException()
        {
            var mockData = new Mock<IBookingService>();

            mockData.Setup(x => x.CreateAsync(It.IsAny<BookingBindingModel>()))
                .ThrowsAsync(new ApplicationException("Nights must be positive"));

            var controller = new BaseConfiguration().WithBookingService(mockData.Object).BuildBookingsController();

            var result =await Assert.ThrowsAsync<ApplicationException>(()=> controller.Post(new BookingBindingModel{Nights = -1}));

            result.Should().BeOfType<ApplicationException>().Which.Message.Should().Be("Nights must be positive");
            
        }
        
        [Fact]
        public async Task BookingService_Create_WhenValidModelSend_OkResult()
        {
            var mockData = new Mock<IBookingService>();
           
            var booking = new BookingBindingModel {Nights = 3, RentalId = 1, Start = DateTime.Today};
            
            mockData.Setup(x => x.CreateAsync(It.IsAny<BookingBindingModel>()))
                .ReturnsAsync(new ResourceIdViewModel{Id = 1});
            
            var controller = new BaseConfiguration().WithBookingService(mockData.Object).BuildBookingsController();
           
            var result = await controller.Post(booking);
            
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<ResourceIdViewModel>()
                .Which.Id.Should().Be(1);
        }
    }
}