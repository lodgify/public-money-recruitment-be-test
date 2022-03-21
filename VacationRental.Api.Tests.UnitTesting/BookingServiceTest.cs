using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services;
using Xunit;

namespace VacationRental.Api.Tests.UnitTesting
{
	public class BookingServiceTest
	{
		public BookingServiceTest()
		{

		}

		[Fact]
		public void Should_Return_Null_On_Non_Existing_Booking()
		{
			var bookingRepoMock = new Mock<GIRepository<Booking>>();
			var bookingService = new BookingService(bookingRepoMock.Object);

			Assert.Null(bookingService.GetById(1));
		}

		[Fact]
		public async void Should_Return_Same_Booking_Object_On_Add()
		{
			var bookingRepoMock = new Mock<GIRepository<Booking>>();
			var bookingService = new BookingService(bookingRepoMock.Object);

			var bookingModel = new Booking()
			{
				RentalId = 1,
				Start = DateTime.Now,
				Nights = 1,
				Unit = 1,
				IsPreparationTime = false
			};

			var result = await bookingService.Add(bookingModel, 3, 2);

			Assert.Equal(bookingModel.RentalId, result.RentalId);
			Assert.Equal(bookingModel.Start, result.Start);
			Assert.Equal(bookingModel.Nights, result.Nights);
			Assert.Equal(bookingModel.Unit, result.Unit);
			Assert.Equal(bookingModel.IsPreparationTime, result.IsPreparationTime);
		}
	}
}
