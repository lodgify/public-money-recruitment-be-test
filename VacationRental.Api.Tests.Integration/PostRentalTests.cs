
using System.Net;
using System.Threading.Tasks;
using VacationRental.WebAPI.DTOs;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
	[Collection("Integration")]
	public class PostRentalTests
	{
		private readonly Request _request;

		public PostRentalTests(IntegrationFixture fixture)
		{
			_request = fixture.Request;
		}

		[Fact]
		public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
		{
			var request = new RentalRequestDTO
			{
				Units = 25,
				PreparationTimeInDays = 1
			};

			ResourceIdViewModel resourceIdResponse = await _request.Post<RentalRequestDTO, ResourceIdViewModel>("rentals", request);
			RentalViewModel rentalResponse = await _request.Get<RentalViewModel>($"rentals/{resourceIdResponse.Id}");
			Assert.Equal(request.Units, rentalResponse.Units);
			Assert.Equal(request.PreparationTimeInDays, rentalResponse.PreparationTimeInDays);
		}

		[Fact]
		public async Task Should_Return_Bad_Request_On_Negative_Units()
		{
			var request = new RentalRequestDTO
			{
				Units = -1,
				PreparationTimeInDays = 1
			};

			var response = await _request.Post("rentals", request);
			Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
		}
	}
}
