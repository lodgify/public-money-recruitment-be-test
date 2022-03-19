using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Domain.Interfaces;
using VacationRental.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/bookings")]
	[ApiController]
	public class BookingsController : ControllerBase
	{
		private readonly IBookingService bookingService;
		private readonly IRentalBookingService rentalBookingService;

		private readonly IMapper mapper;
		private readonly ILogger<BookingsController> logger;

		public BookingsController(IBookingService bookingService,
								  IRentalBookingService rentalBookingService,
								  IMapper mapper,
								  ILogger<BookingsController> logger)
		{
			this.bookingService = bookingService;
			this.rentalBookingService = rentalBookingService;
			this.mapper = mapper;
			this.logger = logger;
		}
		// GET
		// api/v1/Warehouse/StockItem

		/// <summary>
		/// Retrieves booking items
		/// </summary>
		/// <param name="bookingId">bookingId</param>
		/// <returns>A response with stock items list</returns>
		/// <response code="200">Returns the stock items list</response>
		/// <response code="404">Not found</response>
		[HttpGet("{bookingId:int}")]
		[ProducesResponseType(typeof(BookingViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<BookingViewModel> Get(int bookingId)
		{
			if (bookingId <= 0)
				throw new HttpException(HttpStatusCode.BadRequest, "bookingId must be positive");

			var model = bookingService.GetById(bookingId);
			if (model == null)
			{
				return NotFound("Booking not found");
			}
			var response = mapper.Map<Booking, BookingViewModel>(model);

			logger.LogInformation($"GET booking '{model.Id}'");

			return Ok(response);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ResourceIdViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<ResourceIdViewModel>> Post(BookingBindingModel bookingRequestDTO)
		{
			if (bookingRequestDTO.Nights <= 0)
				throw new HttpException(HttpStatusCode.BadRequest, "Nigts must be positive");
			
			//Map model
			var model = mapper.Map<BookingBindingModel, Booking>(bookingRequestDTO);

			//Add to db
			var addedBooking = await rentalBookingService.AddBooking(model);

			var response = mapper.Map<Booking, ResourceIdViewModel>(addedBooking);

			logger.LogInformation($"POST booking '{response.Id}'");

			return Ok(response);
		}
	}
}
