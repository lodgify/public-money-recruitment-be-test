using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Application.Handlers.Rentals;
using VacationRental.Domain.Entities;
using VacationRental.Persistance.Interfaces;
using Xunit;

namespace VacationRental.Api.Tests.Handlers.Rentals
{
    public class UpdateRentalHandlerTest
    {
        [Fact(DisplayName = "HandleUpdateWithNotValidRentalDataShouldReturnNotification")]
        public async Task HandleUpdateWithNotValidRentalDataShouldReturnNotification()
        {
            //Arrange
            var rental = new RentalEntity() { Id = 1, Units = 1, PreparationTimeInDays = 1 };
            var mockedRentalRepository = new Mock<IRepository<RentalEntity>>();
            var mockedBookingRepository = new Mock<IRepository<BookingEntity>>();

            //Act
            var request = new UpdateRentalRequest(rental.Id, rental.Units, rental.PreparationTimeInDays);
            var testObject = new UpdateRentalHandler(mockedRentalRepository.Object, mockedBookingRepository.Object);
            var result = await testObject.Handle(request, new System.Threading.CancellationToken());

            //Assert
            Assert.True(result.Notifications.Count == 1);
            Assert.True(result.Invalid);

        }

        [Fact(DisplayName = "HandleUpdateWithValidDataShouldWork")]
        public async Task HandleUpdateWithValidDataShouldWork()
        {
            //Arrange
            var rental = new RentalEntity() { Id = 1, Units = 1, PreparationTimeInDays = 1 };
            var mockedRentalRepository = new Mock<IRepository<RentalEntity>>();
            var mockedBookingRepository = new Mock<IRepository<BookingEntity>>();


            mockedRentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(await Task.FromResult(rental));

            //Act
            var request = new UpdateRentalRequest(rental.Id, 1, 1);
            var testObject = new UpdateRentalHandler(mockedRentalRepository.Object, mockedBookingRepository.Object);
            var result = await testObject.Handle(request, new System.Threading.CancellationToken());

            //Assert
            Assert.True(result.Notifications.Count == 0);
            Assert.True(result.Valid);

        }

        [Fact(DisplayName = "HandleUpdateWithInvalidUnitShouldThrowExc")]
        public async Task HandleUpdateWithInvalidUnitShouldThrowExc()
        {
            //Arrange
            var rental = new RentalEntity() { Id = 1, Units = 2, PreparationTimeInDays = 1 };
            var mockedRentalRepository = new Mock<IRepository<RentalEntity>>();
            var mockedBookingRepository = new Mock<IRepository<BookingEntity>>();


            mockedRentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(await Task.FromResult(rental));

            //Act
            var request = new UpdateRentalRequest(rental.Id, 1, rental.PreparationTimeInDays);
            var testObject = new UpdateRentalHandler(mockedRentalRepository.Object, mockedBookingRepository.Object);
            var result = await testObject.Handle(request, new System.Threading.CancellationToken());

            //Assert
            Assert.True(result.Notifications.Count == 1);
            Assert.True(result.Invalid);

        }


        [Fact(DisplayName = "HandleUpdateWithOverlappingShouldThrowExc")]
        public async Task HandleUpdateWithOverlappingShouldThrowExc()
        {
            var actualDate = DateTime.Now.Date;

            //Arrange
            var rental = new RentalEntity() { Id = 1, Units = 1, PreparationTimeInDays = 1 };
            
            List<BookingEntity> bookingList = new List<BookingEntity>();

            bookingList.Add(new BookingEntity() { Id = 1, Nights = 1, Start = actualDate, RentalId = 1 });
            bookingList.Add(new BookingEntity() { Id = 1, Nights = 1, Start = actualDate.AddDays(2), RentalId = 1 });

            var mockedRentalRepository = new Mock<IRepository<RentalEntity>>();
            var mockedBookingRepository = new Mock<IRepository<BookingEntity>>();


            mockedRentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(await Task.FromResult(rental));

            mockedBookingRepository.Setup(x => x.GetAll())
           .Returns(bookingList);

            //Act
            var request = new UpdateRentalRequest(rental.Id, 1, 2);
            var testObject = new UpdateRentalHandler(mockedRentalRepository.Object, mockedBookingRepository.Object);
            var result = await testObject.Handle(request, new System.Threading.CancellationToken());

            //Assert
            Assert.True(result.Notifications.Count == 1);
            Assert.True(result.Invalid);

        }
    }
}
