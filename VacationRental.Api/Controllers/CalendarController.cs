using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services;
using VacationRental.Domain.Interfaces;
using VacationRental.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/calendar")]
	[ApiController]
	public class CalendarController : ControllerBase
	{
		private readonly ICalendarService calendarService;

		private readonly IMapper mapper;
		private readonly ILogger<CalendarController> logger;

		public CalendarController(ICalendarService calendarService,
								  IMapper mapper,
								  ILogger<CalendarController> logger)
		{
			this.calendarService = calendarService;
			this.mapper = mapper;
			this.logger = logger;
		}
		// GET
		// api/v1/calendar

		/// <summary>
		/// Retrieves booking items
		/// </summary>
		/// <param name="RentalId">RentalId</param>
		/// <param name="start">Start date</param>
		/// <param name="Nights">Nights</param>
		/// <returns>A response with booking item</returns>
		/// <response code="200">Returns the stock items list</response>
		/// <response code="404">Not found</response>
		/// <response code="400">Bad request</response>
		[HttpGet]
		[ProducesResponseType(typeof(CalendarResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<CalendarResponse> Get([FromQuery] CalendarRequestDTO calendarRequest)
		{
			//Map model
			var model = mapper.Map<CalendarRequestDTO, CalendarRequest>(calendarRequest);

			if (model.Nights <= 0)
				throw new HttpException(HttpStatusCode.BadRequest, "Nights must be positive");

			if (model.RentalId <= 0)
				throw new HttpException(HttpStatusCode.BadRequest, "RentalId must be positive");

			var calendar = calendarService.Get(model);

			var calendarResponse = mapper.Map<Calendar, CalendarResponse>(calendar);

			logger.LogInformation($"GET calendar with rental '{calendarResponse.RentalId}' and date '{calendarResponse.Dates}'");

			return Ok(calendarResponse);
		}
	}
}