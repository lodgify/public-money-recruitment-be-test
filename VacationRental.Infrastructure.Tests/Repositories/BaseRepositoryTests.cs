using VacationRental.Domain.Models.Rentals;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Infrastructure.Tests.Repositories
{
    public class BaseRepositoryTests : IDisposable, IClassFixture<BaseRepositoryTests>
    {
        private BaseRepository<Rental> _testRepository;

        public BaseRepositoryTests()
        {
            _testRepository = new();
        }

        [Fact]
        public void Add_Should_CreateNewEntityInPersitence()
        {
            //Arrange
            var rental = TestData.CreateRentalForTest();

            //Act
            var result = _testRepository.Add(rental);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Rental>(result);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public void Delete_Should_DeleteNewEntity_WhenEntityExistsInPersistence()
        {
            //Arrange
            var rental = TestData.CreateRentalForTest();
            var rentalInPersistence = _testRepository.Add(rental);          
            
            //Act
            _testRepository.Delete(rental);

            rentalInPersistence = _testRepository.GetById(rentalInPersistence.Id);

            //Assert
            Assert.Null(rentalInPersistence);            
        }

        [Fact]
        public void Delete_Should_DoNothing_WhenEntityDoesntExistInPersistence()
        {
            //Arrange
            var rental = TestData.CreateRentalForTest();
            var rentalInPersistence1 = _testRepository.Add(TestData.CreateRentalForTest());
            var rentalInPersistence2 = _testRepository.Add(TestData.CreateRentalForTest());

            //Act
            _testRepository.Delete(rental);

            rentalInPersistence1 = _testRepository.GetById(rentalInPersistence1.Id);
            rentalInPersistence2 = _testRepository.GetById(rentalInPersistence2.Id);

            //Assert
            Assert.NotNull(rentalInPersistence1);
            Assert.NotNull(rentalInPersistence2);
        }

        [Fact]
        public void GetAll_Should_ReturnAllEntitiesFromPersistence()
        {
            //Arrange            
            _testRepository.Add(TestData.CreateRentalForTest());
            _testRepository.Add(TestData.CreateRentalForTest());

            //Act
            var result = _testRepository.GetAll();
            
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }

        [Fact]
        public void GetById_Should_ReturnTheRequestedEntity_WhenExistsInPersitence()
        {
            //Arrange            
            var rentalInDatabase = _testRepository.Add(TestData.CreateRentalForTest());            

            //Act
            var result = _testRepository.GetById(rentalInDatabase.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, rentalInDatabase.Id);
        }

        [Fact]
        public void GetById_Should_ReturnNull_WhenEntityDoesntExistsInPersitence()
        {
            //Act
            var result = _testRepository.GetById(-2);

            //Assert
            Assert.Null(result);            
        }

        [Fact]
        public void Update_Should_ThrowNull_WhenEntityDoesntExistsInPersitence()
        {
            //Arrange 
            var rental = TestData.CreateRentalForTest();

            //Act
            var result = _testRepository.Update(rental);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Update_Should_ReturnTheModifiedEntity_WhenExistsInPersitence()
        {
            //Arrange            w
            var rentalInDatabase = _testRepository.Add(TestData.CreateRentalForTest());

            rentalInDatabase.SetPreparationTimeInDays(3);
            rentalInDatabase.SetUnits(4);

            //Act
            var result = _testRepository.Update(rentalInDatabase);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(rentalInDatabase.Units, result.Units);
            Assert.Equal(rentalInDatabase.PreparationTimeInDays, result.PreparationTimeInDays);
        }


        public void Dispose()
        {
            _testRepository.Dispose();
        }
    }
}
