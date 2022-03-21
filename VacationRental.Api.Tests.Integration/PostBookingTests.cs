
using System;
using System.Net;
using System.Threading.Tasks;
using VacationRental.WebAPI.DTOs;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
	[Collection("Integration")]
	public class PostBookingTests
	{
		private readonly Request _request;

		public PostBookingTests(IntegrationFixture fixture)
		{
			_request = fixture.Request;
		}

		[Fact]
		public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
		{
			//Post Rental
			var postRentalRequest = new RentalRequestDTO { Units = 2 };
			ResourceIdViewModel postRentalResult = await _request.Post<RentalRequestDTO, ResourceIdViewModel>("rentals", postRentalRequest);

			//Post Booking
			var postBookingRequest = new BookingBindingModel
			{
				RentalId = postRentalResult.Id,
				Nights = 2,
				Start = new DateTime(2022, 03, 19)
			};
			ResourceIdViewModel postBookingResult = await _request.Post<BookingBindingModel, ResourceIdViewModel>("bookings", postBookingRequest);

			//Get Booking
			var getBookingResponse = await _request.Get<BookingBindingModel>($"bookings/{postBookingResult.Id}");
			Assert.Equal(postBookingRequest.RentalId, getBookingResponse.RentalId);
			Assert.Equal(postBookingRequest.Nights, getBookingResponse.Nights);
			Assert.Equal(postBookingRequest.Start, getBookingResponse.Start);
		}

		[Fact]
		public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
		{
			//Add Rental
			var postRentalRequest = new RentalRequestDTO { Units = 1, PreparationTimeInDays = 1 };
			ResourceIdViewModel postRentalResult = await _request.Post<RentalRequestDTO, ResourceIdViewModel>("rentals", postRentalRequest);

			//Add same Booking twice
			var postBookingRequest = new BookingBindingModel { RentalId = postRentalResult.Id, Nights = 1, Start = new DateTime(2022, 03, 19) };
			var postBookingResponse = await _request.Post("bookings", postBookingRequest);
			Assert.True(postBookingResponse.IsSuccessStatusCode);

			postBookingResponse = await _request.Post("bookings", postBookingRequest);
			Assert.True(postBookingResponse.StatusCode == HttpStatusCode.BadRequest);
		}
	}
}
