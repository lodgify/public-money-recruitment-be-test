using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Rentals.Commands.CreateRental;
using VacationRental.Application.Features.Rentals.Commands.UpdateRental;
using VacationRental.Application.Features.Rentals.Queries.GetRental;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Rentals;
using ValidationException = VacationRental.Application.Exceptions.ValidationException;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IQueryHandler<GetRentalQuery, RentalDto> _getRentalQueryHandler;
        private readonly ICommandHandler<CreateRentalCommand, ResourceId> _createRentalCommandHandler;
        private readonly ICommandHandler<UpdateRentalCommand, RentalDto> _updateRentalCommandHandler;
        private readonly FluentValidation.IValidator<GetRentalQuery> _getRentalQueryValidator;
        private readonly FluentValidation.IValidator<CreateRentalCommand> _createRentalCommandValidator;
        private readonly FluentValidation.IValidator<UpdateRentalCommand> _updateRentalCommandValidator;

        public RentalsController(IQueryHandler<GetRentalQuery, RentalDto> getRentalQueryHandler, ICommandHandler<CreateRentalCommand, ResourceId> createRentalCommandHandler, ICommandHandler<UpdateRentalCommand, RentalDto> updateRentalCommandHandler, IValidator<GetRentalQuery> getRentalQueryValidator, IValidator<CreateRentalCommand> createRentalCommandValidator, IValidator<UpdateRentalCommand> updateRentalCommandValidator)
        {
            _getRentalQueryHandler = getRentalQueryHandler;
            _createRentalCommandHandler = createRentalCommandHandler;
            _updateRentalCommandHandler = updateRentalCommandHandler;
            _getRentalQueryValidator = getRentalQueryValidator;
            _createRentalCommandValidator = createRentalCommandValidator;
            _updateRentalCommandValidator = updateRentalCommandValidator;
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

        [HttpPut]
        [Route("{id:int}")]
        public RentalDto Update(int id, UpdateRentalRequest request)
        {
            var command = new UpdateRentalCommand(id, request.Units, request.PreparationTimeInDays);
            var result = _updateRentalCommandValidator.Validate(command);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return _updateRentalCommandHandler.Handle(command);
        }
    }
}
