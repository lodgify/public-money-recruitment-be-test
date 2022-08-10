using System;
using System.Collections.Generic;
using System.Text;
using DeepEqual.Syntax;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using VacationRental.Api.Controllers;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Api.Tests.Stubs;

namespace VacationRental.Api.Tests.Units
{
    public class RentalControllerTests
    {
        private IRentalService _rentalService;
        private RentalsController _rentalsController;

        [SetUp]
        public void Setup()
        {
            _rentalService = A.Fake<IRentalService>();
            _rentalsController = new RentalsController(_rentalService);
        }

        [Test]
        public void ShouldReturnNotFoundIfRentalIsNotExistOnGet()
        {
            A.CallTo(() => _rentalService.Get(A<int>._)).Returns(null);

            var result = _rentalsController.Get(3) as StatusCodeResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public void ShouldReturnRentalViewModelIfRentalIsExistOnGet()
        {
            var expectedResult = RentalStubs.SingleRental();
            A.CallTo(() => _rentalService.Get(A<int>._)).Returns(expectedResult);

            var result = _rentalsController.Get(3) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            result.Value.ShouldDeepEqual(expectedResult);
        }

        [Test]
        public void ShouldReturn500StatusCodeIfRentalWasNotCreatedOnPost()
        {
            A.CallTo(() => _rentalService.Create(A<RentalBindingModel>._)).Returns(0);

            var result = _rentalsController.Post(RentalStubs.RentalBindingModel()) as StatusCodeResult;


            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public void ShouldReturnResourceIdViewModelIfRentalCreateSuccessfullyOnPost()
        {
            var expectedResult = 1;
            A.CallTo(() => _rentalService.Create(A<RentalBindingModel>._)).Returns(expectedResult);

            var result = _rentalsController.Post(RentalStubs.RentalBindingModel()) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            result.Value.ShouldDeepEqual(expectedResult);
        }

        [Test]
        public void ShouldReturn204StatusCodeIfRentalUpdateSuccessfullyOnPut()
        {
            var result = _rentalsController.Put(3, RentalStubs.RentalBindingModel()) as StatusCodeResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }
    }
}
