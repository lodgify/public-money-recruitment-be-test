using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Business;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Models;
using Xunit;

namespace VacationRental.Domain.Tests
{
    public class RentalServiceTests
    {
        [Fact]
        public async Task InsertNewRentalObtainRentalId_InsertUpdateDbNoRowsAffected()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();

            var rentalId = 0;
            var rentalEntity = new RentalEntity();
            rentalRepositoryMock.Setup(a => a.InsertNewRentalObtainRentalId(rentalEntity)).Returns(Task.FromResult(rentalId));

            var rentalService = new RentalService(rentalRepositoryMock.Object, null);

            var result = await rentalService.InsertNewRentalObtainRentalId(rentalEntity);

            Assert.Equal(InsertUpdateNewRentalStatus.InsertUpdateDbNoRowsAffected, result.Item1);
            Assert.Equal(rentalId, result.Item2);
        }

        [Fact]
        public async Task InsertNewRentalObtainRentalId_OK()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();

            var rentalId = 1;
            var rentalEntity = new RentalEntity();
            rentalRepositoryMock.Setup(a => a.InsertNewRentalObtainRentalId(rentalEntity)).Returns(Task.FromResult(rentalId));

            var rentalService = new RentalService(rentalRepositoryMock.Object, null);

            var result = await rentalService.InsertNewRentalObtainRentalId(rentalEntity);

            Assert.Equal(InsertUpdateNewRentalStatus.OK, result.Item1);
            Assert.Equal(rentalId, result.Item2);
        }

        [Fact]
        public async Task GetRentalPreparationTimeInDays()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();

            var rentalId = 1;
            var preparationInDays = 2;
            rentalRepositoryMock.Setup(a => a.GetRentalPreparationTimeInDays(rentalId)).Returns(Task.FromResult(preparationInDays));

            var rentalService = new RentalService(rentalRepositoryMock.Object, null);

            var result = await rentalService.GetRentalPreparationTimeInDays(rentalId);

            Assert.Equal(preparationInDays, result);
        }

        [Fact]
        public async Task GetRentalById()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();

            var rentalId = 1;
            var rentalEntity = new RentalEntity();
            rentalRepositoryMock.Setup(a => a.GetRentalById(rentalId)).Returns(Task.FromResult(rentalEntity));

            var rentalService = new RentalService(rentalRepositoryMock.Object, null);

            var result = await rentalService.GetRentalById(rentalId);

            Assert.Equal(rentalEntity, result);
        }

        [Fact]
        public async Task RentalExists()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();

            var rentalId = 1;
            var rentalExists = true;
            rentalRepositoryMock.Setup(a => a.RentalExists(rentalId)).Returns(Task.FromResult(rentalExists));

            var rentalService = new RentalService(rentalRepositoryMock.Object, null);

            var result = await rentalService.RentalExists(rentalId);

            Assert.Equal(rentalExists, result);
        }

        [Fact]
        public async Task UpdateRental_RentalNotExists()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();

            var vacationRentalModel = new VacationalRentalModel();
            var rentalExists = false;
            rentalRepositoryMock.Setup(a => a.RentalExists(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalExists));

            var rentalService = new RentalService(rentalRepositoryMock.Object, null);
            
            var result = await rentalService.UpdateRental(vacationRentalModel);

            Assert.Equal(InsertUpdateNewRentalStatus.RentalNotExists, result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task UpdateRental_UnitsQuantityBookedAlready()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var rentalExists = true;
            var rentalId = 1;
            var units = 2;

            var vacationRentalModel = new VacationalRentalModel { RentalId = rentalId, Units = 1 };
            var rentalEntity = new RentalEntity { Id = rentalId, PreprationTimeInDays = 1, Units = units };

            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 07),
                    Unit = 1
                },
                new BookingEntity
                {
                    Id = 2,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 08),
                    Unit = 2
                }
            };

            rentalRepositoryMock.Setup(a => a.RentalExists(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalExists));
            rentalRepositoryMock.Setup(a => a.GetRentalById(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalEntity));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));

            var rentalService = new RentalService(rentalRepositoryMock.Object, bookingRepositoryMock.Object);

            var result = await rentalService.UpdateRental(vacationRentalModel);

            Assert.Equal(InsertUpdateNewRentalStatus.UnitsQuantityBookedAlready, result.Item1);
            Assert.Equal(2, result.Item2.UnitsBooked);
        }

        [Fact]
        public async Task UpdateRental_DatesOverlapping()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var rentalExists = true;
            var rentalId = 1;
            var units = 2;

            var vacationRentalModel = new VacationalRentalModel { RentalId = rentalId, Units = units };
            var rentalEntity = new RentalEntity { Id = rentalId, PreprationTimeInDays = 1, Units = units };

            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 07),
                    Unit = 1
                },
                new BookingEntity
                {
                    Id = 2,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 08),
                    Unit = 2
                }
            };

            rentalRepositoryMock.Setup(a => a.RentalExists(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalExists));
            rentalRepositoryMock.Setup(a => a.GetRentalById(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalEntity));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));

            var rentalService = new RentalService(rentalRepositoryMock.Object, bookingRepositoryMock.Object);

            var result = await rentalService.UpdateRental(vacationRentalModel);

            Assert.Equal(InsertUpdateNewRentalStatus.DatesOverlapping, result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task UpdateRental_InsertUpdateDbNoRowsAffected()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var rentalExists = true;
            var rentalId = 1;
            var units = 2;
            var rowsAffected = 0;

            var vacationRentalModel = new VacationalRentalModel { RentalId = rentalId, Units = units };
            var rentalEntity = new RentalEntity { Id = rentalId, PreprationTimeInDays = 1, Units = units };

            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 07),
                    Unit = 1
                },
                new BookingEntity
                {
                    Id = 2,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 10),
                    Unit = 2
                }
            };

            rentalRepositoryMock.Setup(a => a.RentalExists(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalExists));
            rentalRepositoryMock.Setup(a => a.GetRentalById(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalEntity));
            rentalRepositoryMock.Setup(a => a.UpdateRental(rentalEntity)).Returns(Task.FromResult(rowsAffected));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));

            var rentalService = new RentalService(rentalRepositoryMock.Object, bookingRepositoryMock.Object);

            var result = await rentalService.UpdateRental(vacationRentalModel);

            Assert.Equal(InsertUpdateNewRentalStatus.InsertUpdateDbNoRowsAffected, result.Item1);
            Assert.Null(result.Item2);
        }


        [Fact]
        public async Task UpdateRental_OK()
        {
            var rentalRepositoryMock = new Mock<IRentalsRepository>();
            var bookingRepositoryMock = new Mock<IBookingsRepository>();

            var rentalExists = true;
            var rentalId = 1;
            var units = 2;
            var rowsAffected = 1;

            var vacationRentalModel = new VacationalRentalModel { RentalId = rentalId, Units = units };
            var rentalEntity = new RentalEntity { Id = rentalId, PreprationTimeInDays = 1, Units = units };
            var vacationalRentalModelResult = new VacationalRentalModel
            {
                RentalId = rentalId,
                PreparationTimeInDays = 1,
                Units = units
            };

            IEnumerable<BookingEntity> bookingEntities = new List<BookingEntity>
            {
                new BookingEntity
                {
                    Id = 1,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 07),
                    Unit = 1
                },
                new BookingEntity
                {
                    Id = 2,
                    RentalId = rentalId,
                    Nights = 1,
                    Start = new DateTime(2021, 04, 10),
                    Unit = 2
                }
            };

            rentalRepositoryMock.Setup(a => a.RentalExists(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalExists));
            rentalRepositoryMock.Setup(a => a.GetRentalById(vacationRentalModel.RentalId)).Returns(Task.FromResult(rentalEntity));
            rentalRepositoryMock.Setup(a => a.UpdateRental(It.IsAny<RentalEntity>())).Returns(Task.FromResult(rowsAffected));
            bookingRepositoryMock.Setup(a => a.GetBookinByRentalId(rentalId)).Returns(Task.FromResult(bookingEntities));

            var rentalService = new RentalService(rentalRepositoryMock.Object, bookingRepositoryMock.Object);

            var result = await rentalService.UpdateRental(vacationRentalModel);

            Assert.Equal(InsertUpdateNewRentalStatus.OK, result.Item1);
            Assert.Equal(vacationalRentalModelResult.PreparationTimeInDays, result.Item2.PreparationTimeInDays);
            Assert.Equal(vacationalRentalModelResult.RentalId, result.Item2.RentalId);
            Assert.Equal(vacationalRentalModelResult.Units, result.Item2.Units);
            Assert.Equal(vacationalRentalModelResult.UnitsBooked, result.Item2.UnitsBooked);
        }
    }
}
