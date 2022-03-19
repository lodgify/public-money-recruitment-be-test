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
	[Route("api/v1/rentals")]
	[ApiController]
	public class RentalsController : ControllerBase
	{
		private readonly IRentalService rentalService;
		private readonly IRentalBookingService rentalBookingService;

		private readonly IMapper mapper;
		private readonly ILogger<RentalsController> logger;



		public RentalsController(IRentalService rentalService,
								 IRentalBookingService rentalBookingService,
								 IMapper mapper,
								 ILogger<RentalsController> logger)
		{
			this.rentalService = rentalService;
			this.mapper = mapper;
			this.logger = logger;
			this.rentalBookingService = rentalBookingService;
		}
		// GET
		// /api/v1/rentals/{rentalId}

		/// <summary>
		/// Retrieves booking items
		/// </summary>
		/// <param name="bookingId">bookingId</param>
		/// <returns>A response with booking item</returns>
		/// <response code="200">Returns the stock items list</response>
		/// <response code="404">Not found</response>
		[HttpGet("{rentalId:int}")]
		[ProducesResponseType(typeof(RentalViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<RentalViewModel> Get(int rentalId)
		{
			var rental = rentalService.GetById(rentalId);
			if (rental == null)
			{
				return NotFound("Rental not found");
			}

			logger.LogInformation($"GET rental '{rental.Id}'");

			return Ok(rental);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ResourceIdViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ResourceIdViewModel>> Post(RentalRequestDTO rentalRequest)
		{
			if (rentalRequest.Units <= 0)
				throw new HttpException(HttpStatusCode.BadRequest, "Units must be positive");
			//No need to validate since is optional
			//if (rentalRequest.PreparationTimeInDays <= 0)
			//	throw new HttpException(HttpStatusCode.BadRequest, "PreparationTimeInDays must be positive");
			//Map model
			var rental = mapper.Map<RentalRequestDTO, Rental>(rentalRequest);
			
			//Add
			var model = await rentalService.AddAsync(rental);
			if (model == null)
			{
				return BadRequest();
			}
			var response = mapper.Map<Rental, ResourceIdViewModel>(model);

			logger.LogInformation($"POST rental '{response.Id}'");

			return Ok(response);
		}

		[HttpPut("{rentalId:int}")]
		[ProducesResponseType(typeof(ResourceIdViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.Conflict)]
		public ActionResult<ResourceIdViewModel> Put(int rentalId, [FromBody] RentalRequestDTO rentalRequest)
		{
			if (rentalId <= 0)
				throw new HttpException(HttpStatusCode.BadRequest, "rentalId must be positive");
			//Map model
			var model = mapper.Map<RentalRequestDTO, Rental>(rentalRequest);
			model.Id = rentalId;

			//Update
			var response = rentalBookingService.UpdateRental(model);

			if (response == null)
			{
				return BadRequest();
			}

			logger.LogInformation($"PUT rental '{response.Id}'");

			return Ok(response);
		}
	}
}
