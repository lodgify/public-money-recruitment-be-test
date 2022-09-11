using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Common.Models;
using AutoMapper;
using FakeItEasy;
using VacationRental.Service.Implementations;
using VacationRental.Repository.Interfaces;
using VacationRental.Service.Interfaces;
using Xunit;

namespace VacationRental.Api.Tests.Rental
{
    public class PostRental
    {
        [Fact]
        public void PostRentalServiceTest()
        {
            //Arrange

            //Act
            var rentalService = A.Fake<IRentalService>();
            var model = A.Fake<RentalViewModel>();
            rentalService.CreateRental(model);
            //Assert
            A.CallTo(() => rentalService.CreateRental(A<RentalViewModel>._)).MustHaveHappenedOnceExactly();
        }

        
    }
}
