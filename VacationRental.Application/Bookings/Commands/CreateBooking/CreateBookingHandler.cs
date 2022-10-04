using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VacationRental.Domain;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, ResourceIdViewModel>
    {
        private readonly VacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateBookingHandler(VacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var rental = await _dbContext.Rentals
                .AsNoTracking()
                .Include(r => r.Units)
                .ThenInclude(u => u.Bookings.Where(b => b.Start < request.Start.AddDays(b.Unit.Rental.PreparationTimeInDays + request.Nights) && request.Start < b.End.AddDays(b.Unit.Rental.PreparationTimeInDays)))
                .FirstOrDefaultAsync(b => b.Id == request.RentalId, cancellationToken);

            if (rental == null)
            {
                throw new ValidationException("Rental not found");
            }

            var availableUnit = rental.Units.FirstOrDefault(u => !u.Bookings.Any());

            if (availableUnit == null)
            {
                throw new ValidationException("Not available");
            }

            var booking = _mapper.Map<Booking>(request);
            booking.UnitId = availableUnit.Id;

            await _dbContext.Bookings.AddAsync(booking, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ResourceIdViewModel>(booking); ;
        }
    }
}