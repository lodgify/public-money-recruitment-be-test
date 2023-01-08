using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Rentals.Commands.CreateRental;
using VacationRental.Application.Features.Rentals.Queries.GetRental;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Rentals;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IQueryHandler<GetRentalQuery, RentalDto> _getRentalQueryHandler;
        private readonly ICommandHandler<CreateRentalCommand, ResourceId> _createRentalCommandHandler;
        private readonly FluentValidation.IValidator<GetRentalQuery> _getRentalQueryValidator;
        private readonly FluentValidation.IValidator<CreateRentalCommand> _createRentalCommandValidator;

        public RentalsController(IQueryHandler<GetRentalQuery, RentalDto> getRentalQueryHandler, ICommandHandler<CreateRentalCommand, ResourceId> createRentalCommandHandler, FluentValidation.IValidator<GetRentalQuery> getRentalQueryValidator, FluentValidation.IValidator<CreateRentalCommand> createRentalCommandValidator)
        {
            _getRentalQueryHandler = getRentalQueryHandler;
            _createRentalCommandHandler = createRentalCommandHandler;
            _getRentalQueryValidator = getRentalQueryValidator;
            _createRentalCommandValidator = createRentalCommandValidator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalDto Get(int rentalId)
        {
            var query = new GetRentalQuery(rentalId);
            var result = _getRentalQueryValidator.Validate(query);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return _getRentalQueryHandler.Handle(query);
        }

        [HttpPost]
        public ResourceId Post(RentalRequest model)
        {
            var command = new CreateRentalCommand(model.Units, model.PreparationTimeInDays);
            var result = _createRentalCommandValidator.Validate(command);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return _createRentalCommandHandler.Handle(command);
        }
    }
}
