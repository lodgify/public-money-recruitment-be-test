using Application.Models.Calendar.Requests;
using Application.Models.Calendar.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace VacationRental.Api.Controllers;

[Route("api/v1/calendar")]
[ApiController]
public class CalendarController : Controller
{
    private readonly IRequestClient<CalendarRequest> _client;

    public CalendarController(IRequestClient<CalendarRequest> client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<ActionResult<CalendarViewModel>> Get([FromQuery] CalendarRequest request)
    {
        if (request != null)
        {
            var response = await _client.GetResponse<CalendarViewModel>(new { RentalId = request.RentalId, Start = request.Start, Nights = request.Nights });
            return response.Message;
        }

        return BadRequest();
    }
   
}