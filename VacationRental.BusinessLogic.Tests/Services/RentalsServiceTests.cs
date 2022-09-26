using System;
using AutoFixture;
using AutoMapper;
using Moq;
using NUnit.Framework;
using VacationRental.BusinessLogic.Mapping;
using VacationRental.BusinessLogic.Services;
using VacationRental.BusinessObjects;
using VacationRental.Repository.Entities;
using VacationRental.Repository.Repositories.Interfaces;

namespace VacationRental.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class RentalsServiceTests
    {
        private Mock<IRentalRepository> _rentalRepositoryMock;
        private RentalsService _rentalsService;

        private Fixture _autoFixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _autoFixture = new Fixture();
        }

        [SetUp]
        public void SetUp()
        {
            var mappingProfile = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RentalsServiceMappingProfile>();
            });
            var mapper = mappingProfile.CreateMapper();
            _rentalRepositoryMock = new Mock<IRentalRepository>(MockBehavior.Strict);

            _rentalsService = new RentalsService(_rentalRepositoryMock.Object, mapper);
        }

        [Test]
        public void GetRental_WhenRentalNotExists_ThenThrowArgumentException()
        {
            // Arrange
            var rentalId = _autoFixture.Create<int>();
            _rentalRepositoryMock.Setup(x => x.GetRentalEntity(rentalId)).Returns((RentalEntity)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _rentalsService.GetRental(rentalId));
            Assert.AreEqual($"Rental with id {rentalId} does not exist.", ex.Message);
            _rentalRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetRental_ThenReturnExistingRental()
        {
            // Arrange
            var rentalId = _autoFixture.Create<int>();
            var rentalEntity = _autoFixture.Create<RentalEntity>();
            _rentalRepositoryMock.Setup(x => x.GetRentalEntity(rentalId)).Returns(rentalEntity);

            // Act
            var result = _rentalsService.GetRental(rentalId);

            // Assert
            Assert.AreEqual(rentalEntity.Id, result.Id);
            Assert.AreEqual(rentalEntity.Units, result.Units);
            _rentalRepositoryMock.VerifyAll();
        }

        [Test]
        public void CreateRental_ThenReturnCreatedEntityId()
        {
            // Arrange
            var rentalId = _autoFixture.Create<int>();
            var createRental = _autoFixture.Create<CreateRental>();
            _rentalRepositoryMock.Setup(x => x.CreateRentalEntity(It.IsAny<RentalEntity>())).Returns(rentalId);

            // Act
            var result = _rentalsService.CreateRental(createRental);

            // Assert
            Assert.AreEqual(rentalId, result);
            _rentalRepositoryMock.VerifyAll();
        }
    }
}
