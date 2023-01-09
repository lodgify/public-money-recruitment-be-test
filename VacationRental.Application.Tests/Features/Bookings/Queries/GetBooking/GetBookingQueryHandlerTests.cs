using AutoMapper;
using Moq;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Bookings.Queries.GetBooking;
using VacationRental.Application.Mapping;
using VacationRental.Domain.Models.Bookings;

namespace VacationRental.Application.Tests.Features.Bookings.Queries.GetBooking
{
    public class GetBookingQueryHandlerTests
    {
        private Mock<IBookingRepository> _bookingRepository;
        private IMapper _mapper;

        public GetBookingQueryHandlerTests()
        {
            _bookingRepository = new();
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Handle_Should_ReturnNotFoundException_WhenBookingDoesntExist()
        {
            //Arrange
            var request = new GetBookingQuery(1);
            var handler = new GetBookingQueryHandler(_bookingRepository.Object, _mapper);
            Booking booking = null;

            //Act
            _bookingRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(booking);

            //Assert
            Assert.Throws<NotFoundException>(() => handler.Handle(request));

        }

        [Fact]
        public void Handle_Should_ReturnABooking_WhenBookingExists()
        {
            //Arrange
            var request = new GetBookingQuery(1);
            var handler = new GetBookingQueryHandler(_bookingRepository.Object, _mapper);
            Booking booking = TestData.CreateBookingForTest(1 , DateTime.Now);

            //Act
            _bookingRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(booking);
            
            var result = handler.Handle(request);
            //Assert

            Assert.NotNull(result);
            Assert.Equal(result.Id, booking.Id);
            Assert.Equal(result.RentalId, booking.RentalId);
            Assert.Equal(result.Start, booking.Start);
            Assert.Equal(result.Nights, booking.Nights);
            Assert.Equal(result.Units, booking.Units);

        }
    }
}
