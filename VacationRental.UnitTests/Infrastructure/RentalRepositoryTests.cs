using System.Threading.Tasks;
using FakeItEasy;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Repositories;
using VacationRental.Infrastructure.Persist.Storage;
using Xunit;

namespace VacationRental.UnitTests.Infrastructure
{
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
            var fake = A.Fake<IInMemoryDataStorage<RentalDataModel>>();

            var id = 1;
            var units = 1;
            var preparationTime = 1;
            var rentalDataModel = new RentalDataModel {Id = id, PreparationTimeInDays = preparationTime, Units = units};
            A.CallTo(() => fake.TryGetValue(id, out rentalDataModel)).Returns(true);

            var repository = new RentalRepository(fake);

            var rental = await repository.Get(new RentalId(id));

            Assert.Equal(new RentalId(1), rental.Id);
            Assert.Equal(preparationTime, rental.PreparationTimeInDays);
            Assert.Equal(units, rental.Units);
        }
    }
}
