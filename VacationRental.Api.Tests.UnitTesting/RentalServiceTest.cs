using Moq;
using System;
using VacationRental.Domain;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services;
using Xunit;

namespace VacationRental.Api.Tests.UnitTesting
{
	public class RentalServiceTest
	{
		public RentalServiceTest()
		{

		}

		[Fact]
		public void Should_Return_False_On_Non_Existing_Rental()
		{
			var rentalRepoMock = new Mock<GIRepository<Rental>>();
			var mockRentalService = new RentalService(rentalRepoMock.Object);

			Assert.Null(mockRentalService.GetById(1));
		}

		[Fact]
		public async void Should_Return_Same_Rental_Object_On_Add()
		{
			var rentalRepoMock = new Mock<GIRepository<Rental>>();
			var rentalService = new RentalService(rentalRepoMock.Object);

			var rentalModel = new Rental()
			{
				Units = 1,
				PreparationTimeInDays = 2
			};

			var result = await rentalService.AddAsync(rentalModel);

			Assert.Equal(rentalModel.PreparationTimeInDays, result.PreparationTimeInDays);
			Assert.Equal(rentalModel.Units, result.Units);
		}
	}
}
