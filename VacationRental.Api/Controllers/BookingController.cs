using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using VacationRental.Api.Operations.BookingOperations;

namespace VacationRental.Api.Controllers;

[Route("api/v1/bookings")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingCreateOperation _bookingCreateOperation;
    private readonly IBookingGetOperation _bookingGetOperation;

    public BookingController(
        IBookingCreateOperation bookingCreateOperation,
        IBookingGetOperation bookingGetOperation)
    {
        _bookingCreateOperation = bookingCreateOperation;
        _bookingGetOperation = bookingGetOperation;
    }

    [HttpGet]
    [Route("{bookingId:int}")]
    public BookingViewModel Get(int bookingId)
    {
        var result = _bookingGetOperation.ExecuteAsync(bookingId);

        return result;
    }

    [HttpPost]
    [Route("/")]
    public ResourceIdViewModel Post(BookingBindingViewModel model)
    {
        var result = _bookingCreateOperation.ExecuteAsync(model);

        return result;
    }
}
