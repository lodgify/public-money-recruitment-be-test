using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.UnitTest
{
    public class RentalsControllerTest : BaseConfiguration
    {
        [Fact]
        public void RentalService_Get_WhenInvalidIdSend_ReturnException()
        {
            var mockData = new Mock<IRentalService>();

            mockData.Setup(x => x.GetRental(It.IsAny<int>()))
                .Throws(new ApplicationException("Rental not found"));

            var controller = new BaseConfiguration().WithRentalService(mockData.Object).BuildRentalsController();

            Action act = () => controller.Get(It.IsAny<int>());

            act.Should().Throw<ApplicationException>().WithMessage("Rental not found");
        }

        [Fact]
        public void RentalService_Get_WhenValidIdSend_OkResult()
        {
            var mockData = new Mock<IRentalService>();

            mockData.Setup(x => x.GetRental(It.IsAny<int>())).Returns(new RentalViewModel
                {Id = 1, Units = 2});

            var controller = new BaseConfiguration().WithRentalService(mockData.Object).BuildRentalsController();

            var result = controller.Get(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<RentalViewModel>()
                .Which.Id.Should().Be(1);
        }
        
        [Fact]
        public async Task RentalService_Create_WhenInvalidModelSend_ReturnException()
        {
            var mockData = new Mock<IRentalService>();

            mockData.Setup(x => x.CreateAsync(It.IsAny<RentalBindingModel>()))
                .ThrowsAsync(new ApplicationException("unit must not empty"));

            var controller = new BaseConfiguration().WithRentalService(mockData.Object).BuildRentalsController();

            var result =await Assert.ThrowsAsync<ApplicationException>(()=> controller.Post(null));

            result.Should().BeOfType<ApplicationException>().Which.Message.Should().Be("unit must not empty");
            
        }
        
        
        
        
        [Fact]
        public async Task RentalService_Create_WhenValidModelSend_OkResult()
        {
            var mockData = new Mock<IRentalService>();
           
            var rental = new RentalBindingModel() {Units = 3};
            
            mockData.Setup(x => x.CreateAsync(It.IsAny<RentalBindingModel>()))
                .ReturnsAsync(new ResourceIdViewModel{Id = 1});

            var controller = new BaseConfiguration().WithRentalService(mockData.Object).BuildRentalsController();
           
            var result = await controller.Post(rental);
            
             result.Should().BeOfType<OkObjectResult>()
                 .Which.Value.Should().BeOfType<ResourceIdViewModel>()
                 .Which.Id.Should().Be(1);
            

        }
    }
}