using System.Threading.Tasks;
using FakeItEasy;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Repositories;
using VacationRental.Infrastructure.Persist.Storage;
using Xunit;

namespace VacationRental.UnitTests.Infrastructure
{
    [Collection("Infrastructure")]
    public class RentalRepositoryTests
    {
        [Fact]
        public async Task Get_RequestNotExistingRental_RentalNotFoundExceptionThrown()
        {
            var repository = new RentalRepository(new InMemoryDataStorage<RentalDataModel>(model => model.Id));

            await Assert.ThrowsAsync<RentalNotFoundException>(async ()=> await repository.Get(new RentalId(1)));
        }


        [Fact]
        public async Task Get_RequestExistingRental_RentalReturned()
        {
            var fakeStorage = A.Fake<IInMemoryDataStorage<RentalDataModel>>();

            var id = 1;
            var units = 1;
            var preparationTime = 1;
            var rentalDataModel = new RentalDataModel {Id = id, PreparationTimeInDays = preparationTime, Units = units};
            A.CallTo(() => fakeStorage.TryGetValue(id, out rentalDataModel)).Returns(true);

            var repository = new RentalRepository(fakeStorage);

            //Act
            var rental = await repository.Get(new RentalId(id));

            Assert.Equal(new RentalId(1), rental.Id);
            Assert.Equal(preparationTime, rental.PreparationTimeInDays);
            Assert.Equal(units, rental.Units);
        }

        [Fact]
        public async Task Update_NotExistingRental_RentalNotFoundExceptionThrown()
        {
            var fakeStorage = A.Fake<IInMemoryDataStorage<RentalDataModel>>();
            var empty = new RentalDataModel();
            A.CallTo(() => fakeStorage.TryGetValue(A<int>._, out empty)).Returns(false);

            var repository = new RentalRepository(fakeStorage);

            //Act
            await Assert.ThrowsAsync<RentalNotFoundException>(async () =>
                await repository.Update(new Rental(new RentalId(1), 10, 1)));
        }

        [Fact]
        public async Task Update_ExistingRental_StoredSuccessfully()
        {
            var id = 1;
            var fakeStorage = A.Fake<IInMemoryDataStorage<RentalDataModel>>();
            var modelBeforeUpdating = new RentalDataModel{Id = id, Units = 1, PreparationTimeInDays = 1};

            var units = 10;
            var preparationTime = 2;
            var rental = new Rental(new RentalId(1), units, preparationTime);

            A.CallTo(() => fakeStorage.TryGetValue(id, out modelBeforeUpdating)).Returns(true);

            var repository = new RentalRepository(fakeStorage);

            //Act
            await repository.Update(rental);


            A.CallTo(() => fakeStorage.Update(A<RentalDataModel>.That.Matches(param=>param.Id == id 
                                                                                     && param.Units == units 
                                                                                     && param.PreparationTimeInDays == preparationTime))
                ).MustHaveHappenedOnceExactly();
        }
    }
}
