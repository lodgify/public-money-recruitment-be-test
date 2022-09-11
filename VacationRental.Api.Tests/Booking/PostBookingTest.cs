using AutoMapper;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Common.Models;
using VacationRental.Repository.Interfaces;
using VacationRental.Service.Implementations;
using Xunit;

namespace VacationRental.Api.Tests.Booking
{
    public class PostBookingTest
    {
        [Fact]
        public void BookingService_ShouldReturnModelBack()
        {
            //Arrange
            var model = new BookingViewModel()
            {
                Nights = 5,
                RentalId = 1,
                Start = DateTime.Now,
            };
            var mapper = A.Fake<IMapper>();
            var repository = A.Fake<IBookingRepository>();
            var rental = new RentalViewModel()
            {
                PreparationTimeInDays = 2,
                Units = 3
            };
            //Act
            var service = new BookingService(mapper, repository);
            model = service.SaveBooking(rental, model);
            //Assert
            Assert.NotNull(model);
        }
    }
}
