using Application.Models;
using Application.Models.Exceptions;
using Application.Models.Rental.Requests;
using Application.Models.Rental.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VacationRental.Api.Controllers;

[Route("api/v1/rentals")]
[ApiController]
public class RentalsController : Controller
{
    private readonly IBusControl _busControl;
    private readonly IRequestClient<CheckRental> _client;

    public RentalsController(IBusControl buscontrol, IRequestClient<CheckRental> client)
    {
        _busControl = buscontrol;
        _client = client;
    }

    [HttpGet]
    [Route("{rentalId:int}")]
    public async Task<ActionResult<RentalResponse>> Get(int rentalId)
    {
        if (rentalId > 0)
        {
            var response = await _client.GetResponse<RentalResponse>(new { Id = rentalId });
            return Ok(response.Message);
        }
        return BadRequest();
    }

    [HttpPost]
    public async Task<ActionResult<ResourceIdViewModel>> Post(CreateRentalRequest model)
    {
        var endpoint = _busControl.CreateRequestClient<CreateRentalRequest>();
        var result = await endpoint.GetResponse<ResourceIdViewModel, RentalNotFound>(model);

        if (result.Is(out Response<RentalNotFound>? notfoundResponse))
        {
        }
        else if (result.Is(out Response<ResourceIdViewModel>? resourcesVieModel))
        {
            return resourcesVieModel.Message;
        }
       
        return BadRequest();
    }

    [HttpPut]
    public async Task<ActionResult> Put(UpdateRentalRequest updModel)
    {
        var endpoint = _busControl.CreateRequestClient<UpdateRentalRequest>();
        var result = await endpoint.GetResponse<ResourceIdViewModel>(updModel);
        return StatusCode(204);
    }
}