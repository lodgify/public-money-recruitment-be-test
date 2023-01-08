using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Bookings.Commands.CreateBooking;
using VacationRental.Application.Features.Bookings.Queries.GetBooking;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Bookings;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IQueryHandler<GetBookingQuery, BookingDto> _getBookingQueryHandler;
        private readonly ICommandHandler<CreateBookingCommand, ResourceId> _createBookingCommandHandler;
        private readonly FluentValidation.IValidator<GetBookingQuery> _getBookingQueryValidator;
        private readonly FluentValidation.IValidator<CreateBookingCommand> _createBookingCommandValidator;

        public BookingsController(IQueryHandler<GetBookingQuery, BookingDto> getBookingQueryHandler, ICommandHandler<CreateBookingCommand, ResourceId> createBookingCommandHandler, FluentValidation.IValidator<GetBookingQuery> getBookingQueryValidator, FluentValidation.IValidator<CreateBookingCommand> createBookingCommandValidator)
        {
            _getBookingQueryHandler = getBookingQueryHandler;
            _createBookingCommandHandler = createBookingCommandHandler;
            _getBookingQueryValidator = getBookingQueryValidator;
            _createBookingCommandValidator = createBookingCommandValidator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingDto Get(int bookingId)
        {
            var query = new GetBookingQuery(bookingId);
            var result = _getBookingQueryValidator.Validate(query);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            return _getBookingQueryHandler.Handle(query);
        }

        [HttpPost]
        public ResourceId Post(BookingRequest request)
        {
            var command = new CreateBookingCommand(request.RentalId, request.Start, request.Nights, request.Units);
            var result = _createBookingCommandValidator.Validate(command);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            return _createBookingCommandHandler.Handle(command);
        }
    }
}
