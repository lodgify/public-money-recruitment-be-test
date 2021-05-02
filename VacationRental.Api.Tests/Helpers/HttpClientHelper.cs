using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace VacationRental.Api.Tests.Helpers
{
    internal static class HttpClientHelper
    {
        internal static async Task<Type> Post<Type>(string url, object request, HttpClient client, Action<HttpResponseMessage> responseProcessing = null) 
        {
            using (var postResponse = await client.PostAsJsonAsync(url, request))
            {
                if (responseProcessing != null)
                    responseProcessing.Invoke(postResponse);

                var responce = await postResponse.Content.ReadAsAsync<Type>();
                return responce;
            }
        }        
        
        
        internal static async Task<Type> Get<Type>(string url, HttpClient client, Action<HttpResponseMessage> responseProcessing = null) 
        {
            using (var getResponse = await client.GetAsync(url))
            {
                if (responseProcessing != null)
                    responseProcessing.Invoke(getResponse);

                var responce = await getResponse.Content.ReadAsAsync<Type>();
                return responce;
            }
        }

    }
}
