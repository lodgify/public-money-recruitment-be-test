using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace VacationRental.Api.Tests.Helpers
{
    internal class HttpClientHelper
    {
        private readonly HttpClient client;
        public HttpClientHelper(HttpClient client)
        {
            this.client = client;
        }

        internal async Task<Type> PostAsync<Type>(string url, object request, Action<HttpResponseMessage> responseProcessing = null) 
        {
            using (var postResponse = await client.PostAsJsonAsync(url, request))
            {
                if (responseProcessing != null)
                    responseProcessing.Invoke(postResponse);

                var responce = await postResponse.Content.ReadAsAsync<Type>();
                return responce;
            }
        }        
        
        internal async Task<Type> GetAsync<Type>(string url, Action<HttpResponseMessage> responseProcessing = null) 
        {
            using (var getResponse = await client.GetAsync(url))
            {
                if (responseProcessing != null)
                    responseProcessing.Invoke(getResponse);

                var responce = await getResponse.Content.ReadAsAsync<Type>();
                return responce;
            }
        }

        internal async Task<Type> PutAsync<Type>(string url, object request, Action<HttpResponseMessage> responseProcessing = null) 
        {
            using (var getResponse = await client.PutAsJsonAsync(url, request))
            {
                if (responseProcessing != null)
                    responseProcessing.Invoke(getResponse);

                var responce = await getResponse.Content.ReadAsAsync<Type>();
                return responce;
            }
        }

    }
}
