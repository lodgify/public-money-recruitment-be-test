using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Booking.Responses;
using Application.Models.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace VacationRental.Api.Controllers;

[Route("api/v1/bookings")]
[ApiController]
public class BookingsController : Controller
{
    private readonly IBusControl _busControl;
    private readonly IRequestClient<CheckBooking> _client;
    public BookingsController(IBusControl busControl, IRequestClient<CheckBooking> client)
    {
        _busControl = busControl;
        _client = client;
    }

    [HttpGet]
    [Route("{bookingId:int}")]
    public async Task<ActionResult<BookingResponse>> Get(int bookingId)
    {
        var response = await _client.GetResponse<BookingResponse>(new { Id = bookingId });
        return Ok(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<ResourceIdViewModel>> Post(CreateBookingRequest model)
    {
        var endpoint = _busControl.CreateRequestClient<CreateBookingRequest>();
        var result = await endpoint.GetResponse<ResourceIdViewModel, RentalNotFound>(model);

        if (result.Is(out Response<RentalNotFound>? notfoundResponse))
        {
            return NotFound();
        }
        else if (result.Is(out Response<ResourceIdViewModel>? resourcesVieModel))
        {
            return Ok(resourcesVieModel.Message);
        }
        return BadRequest();
    }
}