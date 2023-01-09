using VacationRental.Domain.Models.Rentals;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Infrastructure.Tests.Repositories
{
    public class BookingRepositoryTests : IDisposable, IClassFixture<BookingRepositoryTests>
    {
        private BookingRepository _bookingRepository;
        private BaseRepository<Rental> _rentalRepository;

        public BookingRepositoryTests() 
        {
            _rentalRepository = new();
            _bookingRepository = new ();
        }

        [Fact]
        public void GetBookingByRentalId_Should_ReturnBookingsRelatedToARental_WhenRentalExistsInPersistence()
        {

            //Arrange
            var rentalInPersitence = _rentalRepository.Add(TestData.CreateRentalForTest());
            _bookingRepository.Add(TestData.CreateBookingForTest(rentalInPersitence.Id, DateTime.Now));
            _bookingRepository.Add(TestData.CreateBookingForTest(rentalInPersitence.Id, DateTime.Now.AddDays(2)));

            //Act
            var result = _bookingRepository.GetBookingByRentalId(rentalInPersitence.Id);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
        }

        public void Dispose()
        {
            _bookingRepository.Dispose();
            _rentalRepository.Dispose();
        }
    }
}
