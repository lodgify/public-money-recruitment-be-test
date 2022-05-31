using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
	[Collection("Integration")]
	public class PostRentalTests
	{
		private readonly HttpClient _client;

		public PostRentalTests(IntegrationFixture fixture)
		{
			_client = fixture.Client;
		}

		[Fact]
		public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
		{
			var request = new RentalBindingModel
			{
				Units = 25,
				PreparationTimeInDays = 2
			};

			ResourceIdViewModel postResult;
			using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
			{
				Assert.True(postResponse.IsSuccessStatusCode);
				postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
			}

			using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
			{
				Assert.True(getResponse.IsSuccessStatusCode);

				var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
				Assert.Equal(request.Units, getResult.Units);
				Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
			}
		}

		[Fact]
		public async Task GivenCompleteRequest_WhenPutBooking_ThenAPutReturnsErrorWhenThereIsOverbooking()
		{
			RentalBindingModel postRentalRequest = new RentalBindingModel
			{
				Units = 1,
				PreparationTimeInDays = 1
			};

			ResourceIdViewModel postRentalResult;
			using (HttpResponseMessage postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
			{
				postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
			}

			BookingBindingModel postBooking1Request = new BookingBindingModel
			{
				RentalId = postRentalResult.Id,
				Nights = 3,
				Start = new DateTime(2002, 01, 01)
			};
			await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request);
			BookingBindingModel postBooking2Request = new BookingBindingModel
			{
				RentalId = postRentalResult.Id,
				Nights = 1,
				Start = new DateTime(2002, 01, 06)
			};
			await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request);

			RentalBindingModel putRentalRequest = new RentalBindingModel
			{
				Id = postRentalResult.Id,
				Units = 1,
				PreparationTimeInDays = 2
			};

			await _client.PutAsJsonAsync($"/api/v1/rentals/{putRentalRequest.Id}", putRentalRequest);
			RentalBindingModel putRentalRequest2 = new RentalBindingModel
			{
				Id = postRentalResult.Id,
				Units = 1,
				PreparationTimeInDays = 3
			};

			await Assert.ThrowsAsync<ApplicationException>(async () =>
			{
				using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{putRentalRequest2.Id}", putRentalRequest2))
				{

				}
			});
		}
	}
}
