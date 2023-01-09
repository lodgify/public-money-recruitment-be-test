using AutoMapper;
using Castle.DynamicProxy.Generators;
using Moq;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Rentals.Queries.GetRental;
using VacationRental.Application.Mapping;
using VacationRental.Domain.Messages.Rentals;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Tests.Features.Rentals.Queries
{
    public class GetRentalQueryHandlerTests
    {
        private Mock<IRepository<Rental>> _rentalRepository;
        private IMapper _mapper;

        public GetRentalQueryHandlerTests()
        {
            _rentalRepository = new();
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Handle_Should_ReturnNotFoundException_WhenRentalDoesntExist()
        {
            //Arrange
            var query = new GetRentalQuery(1);
            var handler = new GetRentalQueryHandler(_rentalRepository.Object, _mapper);
            Rental rental = null;
            
            //Act
            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(rental);

            //Assert
            Assert.Throws<NotFoundException>(() => handler.Handle(query));
        }

        [Fact]
        public void Handle_Should_ReturnRequestedRental_WhenRentalExistInPersitence()
        {
            //Arrange
            var query = new GetRentalQuery(1);
            var handler = new GetRentalQueryHandler(_rentalRepository.Object, _mapper);
            Rental rental = TestData.CreateRentalForTest();

            //Act
            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(rental);
            var result = handler.Handle(query);
            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<RentalDto>(result);            
        }
    }
}
