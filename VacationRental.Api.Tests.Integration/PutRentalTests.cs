
using System;
using System.Net;
using System.Threading.Tasks;
using VacationRental.WebAPI.DTOs;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
	[Collection("Integration")]
	public class PutRentalTests
	{
		private readonly Request _request;

		public PutRentalTests(IntegrationFixture fixture)
		{
			_request = fixture.Request;
		}

		[Fact]
		public async Task Should_Match_Put_Request_And_Get_Response()
		{
			//Add rental
			var request = new RentalRequestDTO
			{
				Units = 25,
				PreparationTimeInDays = 1
			};
			ResourceIdViewModel postResult = await _request.Post<RentalRequestDTO, ResourceIdViewModel>("rentals", request);

			//Get added rental
			RentalViewModel getResult = await _request.Get<RentalViewModel>($"rentals/{postResult.Id}"); ;
			Assert.Equal(request.Units, getResult.Units);
			Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);


			//Update rental
			request.Units = 3;
			request.PreparationTimeInDays = 4;
			ResourceIdViewModel putResult = await _request.Put<RentalRequestDTO, ResourceIdViewModel>($"rentals/{getResult.Id}", request);

			//Get updated rental
			getResult = await _request.Get<RentalViewModel>($"rentals/{putResult.Id}");
			Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
			Assert.Equal(request.Units, getResult.Units);
		}

		[Fact]
		public async Task Should_Return_Conflict_On_Extended_Preparation_Time()
		{
			//Add rental
			var rentalRequest = new RentalRequestDTO
			{
				Units = 1,
				PreparationTimeInDays = 1
			};
			ResourceIdViewModel postResult = await _request.Post<RentalRequestDTO, ResourceIdViewModel>("rentals", rentalRequest);


			//Add a booking
			var bookingRequest = new BookingBindingModel
			{
				RentalId = postResult.Id,
				Nights = 1,
				Start = DateTime.Now.Date
			};
			var postResponse = await _request.Post("bookings", bookingRequest);
			Assert.True(postResponse.IsSuccessStatusCode);


			//Add another booking two days after
			bookingRequest.Start = bookingRequest.Start.Date.AddDays(2);
			postResponse = await _request.Post("bookings", bookingRequest);
			Assert.True(postResponse.IsSuccessStatusCode);


			//Try to update rental
			rentalRequest.PreparationTimeInDays++;
			var putResponse = await _request.Put($"rentals/{postResult.Id}", rentalRequest);
			Assert.True(putResponse.StatusCode == HttpStatusCode.Conflict);

		}

	}
}
