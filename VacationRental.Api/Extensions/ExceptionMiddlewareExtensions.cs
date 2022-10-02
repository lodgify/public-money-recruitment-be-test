using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using VacationRental.Domain.Extensions.Common;
using VacationRental.Domain.Models.Error;
using VacationRental.Domain.VacationRental.Extensions.Common;

namespace VacationRental.Api.Extensions
{
	public static class ExceptionMiddlewareExtensions
	{

		public static void ConfigureExceptionHandler(this WebApplication app)
		{
			app.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					context.Response.ContentType = "application/json";

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
							ConflictException => StatusCodes.Status409Conflict,
							InternalException => StatusCodes.Status500InternalServerError,
							_ => StatusCodes.Status500InternalServerError
						};
						await context.Response.WriteAsync(new ErrorDetails()
						{
							StatusCode = context.Response.StatusCode,
							Message = contextFeature.Error.Message,
						}.ToString());
					}
				});
			});
		}

	}
}
