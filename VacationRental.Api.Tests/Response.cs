using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VacationRental.Services.Models;

namespace VacationRental.Api.Tests
{
    public class Response<T>
    {
        public T Content { get; set; }

        public ErrorInfoDto Error { get; set; }

        public Response()
        {
            Content = default;
            Error = null;
        }

        public async Task Read(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Error = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
            }

            Content = await response.Content.ReadFromJsonAsync<T>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
