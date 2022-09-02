using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;
using VR.Infrastructure.Exceptions;

namespace VacationRental.Api.Infrastructure.Exceptions
{
    public class ErrorResponseHandlingMiddleware
    {
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        private readonly RequestDelegate _next;

        private readonly IHostingEnvironment _env;

        public ErrorResponseHandlingMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            ProblemDetails problemDetails;
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentException ex)
            {
                problemDetails = new ProblemDetails
                {
                    Detail = ex.Message,
                    Title = "Argument Exception",
                    Status = StatusCodes.Status400BadRequest,
                };
                await GenerateResponseAsync(httpContext, problemDetails);
                if (!_env.IsEnvironment("Test"))
                {
                    throw;
                }
            }
            catch (ValidationException ex)
            {
                problemDetails = new ProblemDetails
                {
                    Detail = string.Join(";", ex.Errors.Select(x => x.ErrorMessage)),
                    Title = "Validation Exception",
                    Status = StatusCodes.Status400BadRequest,
                };
                problemDetails.Extensions.Add(nameof(ex.Errors), ex.Errors.Select(x => x.ErrorMessage));

                await GenerateResponseAsync(httpContext, problemDetails);
                if (!_env.IsEnvironment("Test"))
                {
                    throw;
                }
            }
            catch (OperationCanceledException ex)
            {
                problemDetails = new ProblemDetails
                {
                    Detail = ex.Message,
                    Title = "Operation Cancelled Exception",
                    Status = StatusCodes.Status400BadRequest,
                };

                await GenerateResponseAsync(httpContext, problemDetails);
                if (!_env.IsEnvironment("Test"))
                {
                    throw;
                }
            }
            catch (VacationRentalException ex)
            {
                int status;

                switch (ex)
                {
                    case NotFoundException:
                        status = StatusCodes.Status404NotFound;
                        break;
                    case BookingConflictsException:
                        status = StatusCodes.Status409Conflict;
                        break;
                    default:
                        status = StatusCodes.Status400BadRequest;
                        break;
                }

                problemDetails = new ProblemDetails
                {
                    Detail = ex.Details,
                    Title = ex.Title,
                    Status = status,
                };

                await GenerateResponseAsync(httpContext, problemDetails);

                if (!_env.IsEnvironment("Test"))
                {
                    throw;
                }
            }
            catch (Exception)
            {
                problemDetails = new ProblemDetails
                {
                    Detail = "An application error occurred",
                    Title = "Application Error",
                    Status = StatusCodes.Status500InternalServerError,
                };

                await GenerateResponseAsync(httpContext, problemDetails);
                throw;
            }
        }

        private static async Task GenerateResponseAsync(HttpContext httpContext, ProblemDetails problemDetails)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsync(
                JsonConvert.SerializeObject(
                    problemDetails,
                    _jsonSettings),
                httpContext.RequestAborted);
        }
    }
}
