using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VacationRental.Domain;
using VacationRental.Domain.Entities;
using Unit = VacationRental.Domain.Entities.Unit;

namespace VacationRental.Application.Rentals.Commands.UpdateRental
{
    public class UpdateRentalHandler : IRequestHandler<UpdateRentalCommand, ResourceIdViewModel>
    {
        private readonly VacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateRentalHandler(VacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _dbContext.Rentals
                .AsNoTracking()
                .Include(r => r.Units)
                .ThenInclude(u => u.Bookings.Where(b => DateTime.UtcNow < b.End.AddDays(b.Unit.Rental.PreparationTimeInDays)))
                .FirstOrDefaultAsync(b => b.Id == request.RentalId, cancellationToken);

            if (rental == null)
            {
                throw new ValidationException("Rental not found");
            }

            UpdateUnits(rental, request);
            UpdatePreparationTime(rental, request);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ResourceIdViewModel>(rental);
        }

        private void UpdateUnits(Rental rental, UpdateRentalCommand request)
        {
            if (rental.Units.Count(u => u.Bookings.Any()) > request.Units)
            {
                throw new ValidationException("You can not decrease the number of units.");
            }

            if (rental.Units.Count < request.Units)
            {
                var unitsToAdd = Enumerable.Range(0, request.Units - rental.Units.Count).Select(x => new Unit { RentalId = rental.Id });
                _dbContext.Units.AddRange(unitsToAdd);
            }

            if (rental.Units.Count > request.Units)
            {
                var unitsToRemove = rental.Units.Where(u => !u.Bookings.Any()).Take(rental.Units.Count - request.Units);
                _dbContext.Units.RemoveRange(unitsToRemove);
            }
        }

        private void UpdatePreparationTime(Rental rental, UpdateRentalCommand request)
        {
            foreach (var unit in rental.Units.Where(x => x.Bookings.Any()))
            {
                var bookings = unit.Bookings.OrderBy(b => b.Start).ToList();

                for (var i = 0; i < bookings.Count - 1; i++)
                {
                    if (bookings[i].End.AddDays(request.PreparationTimeInDays) >= bookings[i + 1].Start)
                    {
                        throw new ValidationException("Preparation time can not be updated.");
                    }
                }
            }

            rental.PreparationTimeInDays = request.PreparationTimeInDays;

            _dbContext.Rentals.Update(rental);
        }
    }
}
