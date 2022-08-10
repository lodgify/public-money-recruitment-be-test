using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using VacationRental.Api.Models;
using VacationRental.Api.Tests.Stubs;

namespace VacationRental.Api.Tests.Units
{
    public class RentalServiceTests : BaseServiceTests
    {
        [Test]
        public void ShouldReturnRentalIdOnCreation()
        {
            var model = new RentalBindingModel()
            {
                PreparationTimeInDays = 2,
                Units = 3
            };

            A.CallTo(() => RentalRepository.Count).Returns(5);

            var rentalId = RentalService.Create(model);

            Assert.That(rentalId, Is.Not.Null);
            Assert.AreEqual(6, rentalId.Id);
            A.CallTo(() => RentalRepository.Add(A<int>._, A<RentalViewModel>._)).MustHaveHappened();
        }

        [Test]
        public void ShouldReturnCorrectRentalViewModelOnGet()
        {
            int id = 1;
            var expectedResult = new RentalViewModel()
            {
                Id = 1,
                PreparationTimeInDays = 2,
                Units = 3
            };

            A.CallTo(() => RentalRepository.HasValue(id)).Returns(true);
            A.CallTo(() => RentalRepository.Get(id)).Returns(expectedResult);

            var rental = RentalService.Get(id);

            Assert.That(rental, Is.Not.Null);
            Assert.AreEqual(expectedResult, rental);
        }

        [Test]
        public void ShouldThrowApplicationExceptionIfRentalNotFoundOnGet()
        {
            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(false);

            var ex = Assert.Throws<ApplicationException>(() => RentalService.Get(3));

            RentalNotFoundExceptionChecks(ex);
        }

        [Test]
        public void ShouldThrowApplicationExceptionIfRentalNotFoundOnUpdate()
        {
            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(false);

            var ex = Assert.Throws<ApplicationException>(() => RentalService.Update(1, new RentalBindingModel()));

            RentalNotFoundExceptionChecks(ex);
        }

        [TestCaseSource(nameof(UpdateRentalModelHappyPathCases))]
        public void ShouldUpdateRentalModelIfRequestModelIsCorrectOnUpdate(List<BookingViewModel> bookings)
        {
            int id = 1;
            var model = new RentalBindingModel { PreparationTimeInDays = 2, Units = 4 };
            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(true);
            A.CallTo(() => BookingRepository.GetBookingsByRentalId(id)).Returns(bookings);

            RentalService.Update(id, model);

            A.CallTo(() => RentalRepository.Update(A<int>._, A<RentalBindingModel>._)).MustHaveHappened();
        }

        [TestCaseSource(nameof(UpdateRentalModelWithIncorrectDataCases))]
        public void ShouldThrowApplicationExceptionIfRequestModelIsIncorrectOnUpdate(int id, RentalBindingModel model, List<BookingViewModel> bookings)
        {
            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(true);
            A.CallTo(() => BookingRepository.GetBookingsByRentalId(id)).Returns(bookings);

            var ex = Assert.Throws<ApplicationException>(() => RentalService.Update(id, model));

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot update because of booking conflicts"));
        }


        

        public static IEnumerable<TestCaseData> UpdateRentalModelHappyPathCases
        {
            get
            {
                yield return new TestCaseData(Stubs.BookingStubs.BookingsWithoutCrossDays());
                yield return new TestCaseData(Stubs.BookingStubs.EmptyBookingList());
                yield return new TestCaseData(Stubs.BookingStubs.BookingWithCrossDays());
            }
        }

        public static IEnumerable<TestCaseData> UpdateRentalModelWithIncorrectDataCases
        {
            get
            {
                yield return new TestCaseData(1, new RentalBindingModel { PreparationTimeInDays = 2, Units = 2 }, Stubs.BookingStubs.BookingWithCrossDays());
                yield return new TestCaseData(1, new RentalBindingModel { PreparationTimeInDays = 15, Units = 2 }, Stubs.BookingStubs.BookingsWithoutCrossDays());
            }
        }
    }
}
