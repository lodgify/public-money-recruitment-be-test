using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
	[Collection("Integration")]
	public class GetCalendarTests
	{
		private readonly Request _request;

		public GetCalendarTests(IntegrationFixture fixture)
		{
			_request = fixture.Request;
		}

		[Theory]
		[InlineData(1, "2022-03-19", -1)]
		public async Task Should_Return_BadRequest_On_Negative_Nights(int rentalId, string start, int nights)
		{
			var getCalendarResponse = await _request.Get($"calendar?rentalId={rentalId}&start={start}&nights={nights}");
			Assert.True(getCalendarResponse.StatusCode == HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData(123456789, "2022-03-19", 1)]
		public async Task Should_Return_NotFound_On_Non_Existing_Rental(int rentalId, string start, int nights)
		{
			var getCalendarResponse = await _request.Get($"calendar?rentalId={rentalId}&start={start}&nights={nights}");
			Assert.True(getCalendarResponse.StatusCode == HttpStatusCode.NotFound);
		}
	}
}
