using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Services;
using Xunit;

namespace VacationRental.Domain.Tests
{
    public class BookingServiceTests
    {
        [Fact]
        public async Task InsertNewBooking_Response_RentalUnitNotAvailable()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            int rentalId = 1;
            int rentalUnits = 2;
            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    Nights = 3,
                    RentalId = rentalId,
                    Start = DateTime.Now.Date,
                    Unit = rentalUnits
                },
                new BookingEntity
                {
                    Id = 2,
                    Nights = 5,
                    RentalId = rentalId,
                    Start = DateTime.Now.Date,
                    Unit = rentalUnits
                }
            };

            var bookingEntityToInsert = new BookingEntity
            {
                Nights = 1,
                RentalId = rentalId,
                Start = DateTime.Now.Date,
                Unit = rentalUnits
            };

            rentalRepositoryMock.Setup(a => a.GetRentalUnits(rentalId)).Returns(Task.FromResult(rentalUnits));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));

            var bookingService = new BookingService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var result = await bookingService.InsertNewBooking(bookingEntityToInsert);

            Assert.Equal((InsertNewBookingStatus.NotAvailable, 0), result);
        }

        [Fact]
        public async Task InsertNewBooking_Response_InsertDbNoRowsAffected()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            int rentalId = 1;
            int rentalUnits = 2;
            int bookingId = 0;
            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    Nights = 3,
                    RentalId = rentalId,
                    Start = DateTime.Now.Date,
                    Unit = rentalUnits
                }
            };

            var bookingEntityToInsert = new BookingEntity
            {
                Nights = 1,
                RentalId = rentalId,
                Start = DateTime.Now.Date,
                Unit = rentalUnits
            };

            rentalRepositoryMock.Setup(a => a.GetRentalUnits(rentalId)).Returns(Task.FromResult(rentalUnits));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));
            bookingRepositoryMock.Setup(a => a.InsertBooking(bookingEntityToInsert)).Returns(Task.FromResult(bookingId));
            var bookingService = new BookingService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var result = await bookingService.InsertNewBooking(bookingEntityToInsert);

            Assert.Equal((InsertNewBookingStatus.InsertDbNoRowsAffected, 0), result);
        }

        [Fact]
        public async Task InsertNewBooking_Response_OK()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            int rentalId = 1;
            int rentalUnits = 2;
            int bookingId = 2;
            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    Nights = 3,
                    RentalId = rentalId,
                    Start = DateTime.Now.Date,
                    Unit = rentalUnits
                }
            };

            var bookingEntityToInsert = new BookingEntity
            {
                Nights = 1,
                RentalId = rentalId,
                Start = DateTime.Now.Date,
                Unit = rentalUnits
            };

            rentalRepositoryMock.Setup(a => a.GetRentalUnits(rentalId)).Returns(Task.FromResult(rentalUnits));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));
            bookingRepositoryMock.Setup(a => a.InsertBooking(bookingEntityToInsert)).Returns(Task.FromResult(bookingId));
            var bookingService = new BookingService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var result = await bookingService.InsertNewBooking(bookingEntityToInsert);

            Assert.Equal((InsertNewBookingStatus.OK, bookingId), result);
        }

        [Fact]
        public async Task InsertNewBooking_NullReferenceException_ComplexParameterNull()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var bookingService = new BookingService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(async () => await bookingService.InsertNewBooking(null));

            Assert.NotNull(exception);
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task InsertNewBooking_InvalidOperationException_RentalIdZero()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var bookingService = new BookingService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(async () => await bookingService.InsertNewBooking(new BookingEntity { RentalId = 0 }));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public async Task InsertNewBooking_InvalidOperationException_DateTimeStartMinimumValue()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var bookingService = new BookingService(bookingRepositoryMock.Object, rentalRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(async () => await bookingService.InsertNewBooking(new BookingEntity { RentalId = 1, Unit = 1, Start = DateTime.MinValue }));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public async Task GetBookingById()
        {
            var bookingId = 1;
            var bookingEntity = new BookingEntity
            {
                Id = bookingId,
                Nights = 2,
                RentalId = 1,
                Start = DateTime.Now.Date,
                Unit = 3
            };

            var bookingRepositoryMock = new Mock<IBookingsRepository>();
            bookingRepositoryMock.Setup(a => a.GetBookingById(bookingId)).Returns(Task.FromResult(bookingEntity));
            var bookingService = new BookingService(bookingRepositoryMock.Object, null);

            Assert.Equal(bookingEntity, await bookingService.GetBookingById(bookingId));
        }

        [Fact]
        public async Task BookingExists()
        {
            var bookingId = 1;
            var bookingExists = true;

            var bookingRepositoryMock = new Mock<IBookingsRepository>();
            bookingRepositoryMock.Setup(a => a.BookingExists(bookingId)).Returns(Task.FromResult(bookingExists));
            var bookingService = new BookingService(bookingRepositoryMock.Object, null);

            Assert.Equal(bookingExists, await bookingService.BookingExists(bookingId));
        }
    }
}
