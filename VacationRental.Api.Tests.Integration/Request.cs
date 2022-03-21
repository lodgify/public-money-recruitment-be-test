using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
	public class Request
	{
		private readonly HttpClient client;

		public Request(HttpClient client)
		{
			this.client = client;
		}

		public async Task<T2> Post<T1, T2>(string route, T1 request)
		{
			var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

			using (var postResponse = await client.PostAsync($"/api/v1/{route}", requestContent))
			{
				Assert.True(postResponse.IsSuccessStatusCode);
				var postResponseContent = await postResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T2>(postResponseContent);
			}
		}

		public async Task<HttpResponseMessage> Post<T>(string route, T request)
		{
			var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

			using (var postResponse = await client.PostAsync($"/api/v1/{route}", content))
			{
				return postResponse;
			}
		}

		public async Task<T2> Put<T1, T2>(string route, T1 request)
		{
			var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

			using (var putResponse = await client.PutAsync($"/api/v1/{route}", requestContent))
			{
				Assert.True(putResponse.IsSuccessStatusCode);
				var postResponseContent = await putResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T2>(postResponseContent);
			}
		}

		public async Task<HttpResponseMessage> Put<T>(string route, T request)
		{
			var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

			using (var putResponse = await client.PutAsync($"/api/v1/{route}", content))
			{
				return putResponse;
			}
		}

		public async Task<HttpResponseMessage> Get(string route)
		{
			using (var getResponse = await client.GetAsync($"/api/v1/{route}"))
			{
				return getResponse;
			}
		}

		public async Task<T> Get<T>(string route)
		{
			using (var getResponse = await client.GetAsync($"/api/v1/{route}"))
			{
				Assert.True(getResponse.IsSuccessStatusCode);
				var getResponseContent = await getResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(getResponseContent);
			}
		}

	}
}
