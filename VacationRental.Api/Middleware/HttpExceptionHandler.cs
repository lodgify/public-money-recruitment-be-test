using VacationRental.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacationRental.WebAPI.Middleware
{
	internal class HttpExceptionHandler
	{
		private readonly RequestDelegate next;
		private readonly ILogger<HttpExceptionHandler> logger;

		public HttpExceptionHandler(RequestDelegate next, ILogger<HttpExceptionHandler> logger)
		{
			this.next = next;
			this.logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await this.next.Invoke(context);
			}
			catch (HttpException e)
			{
				var response = context.Response;
				if (response.HasStarted)
				{
					throw;
				}

				int statusCode = (int)e.StatusCode;
				if (statusCode >= 500 && statusCode <= 599)
				{
					logger.LogError(e, "Server exception");
				}
				response.Clear();
				response.StatusCode = statusCode;
				response.ContentType = "application/json; charset=utf-8";
				response.Headers[HeaderNames.CacheControl] = "no-cache";
				response.Headers[HeaderNames.Pragma] = "no-cache";
				response.Headers[HeaderNames.Expires] = "-1";
				response.Headers.Remove(HeaderNames.ETag);

				var bodyObj = new
				{
					Message = e.Message,
					Code = e.StatusCode,
					Status = e.StatusCode.ToString()
				};
				var body = JsonSerializer.Serialize(bodyObj);
				await context.Response.WriteAsync(body);
			}
		}
	}
}
