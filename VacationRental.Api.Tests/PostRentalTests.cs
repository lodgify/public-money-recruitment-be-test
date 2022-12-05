using AutoFixture;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Business;
using VacationRental.Business.BusinessLogic;
using VacationRental.Infrastructure.Models;
using VacationRental.Infrastructure.Repositories;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class PostRentalTests
    {
        private IFixture _fixture;
        private RentalBusinessLogic _rentalBusinessLogic;
        public PostRentalTests()
        {
            _fixture = new Fixture();

            _fixture.Inject<IRentalRepository>(new RentalRepository(new Dictionary<int, Rental>()));
            _fixture.Inject<IBookingsRepository>(new BookingRepository(new Dictionary<int, Booking>()));

            var mappingConfig = new MapperConfiguration(
                    mc => mc.AddProfile(new MappingProfile()));
            var mapper = mappingConfig.CreateMapper();

            _fixture.Inject(mapper);
            _rentalBusinessLogic = _fixture.Create<RentalBusinessLogic>();
        }

        [Fact]
        public void GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            // Arrange
            var request = _fixture.Build<RentalBindingModel>()
                .With(m => m.Units, 25)
                .With(m => m.PreparationTimeInDays, 3)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(request);

            // Act
            var rental = _rentalBusinessLogic.GetRental(rentalId);

            // Assert
            Assert.Equal(request.Units, rental.Units);
            Assert.Equal(request.PreparationTimeInDays, rental.PreparationTimeInDays);
        }
    }
}
