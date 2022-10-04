using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VacationRental.Application.Rentals.Models;
using VacationRental.Domain;

namespace VacationRental.Application.Rentals.Queries.GetRental
{
    public class GetRentalsHandler : IRequestHandler<GetRentalsQuery, RentalViewModel>
    {
        private readonly VacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetRentalsHandler(VacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RentalViewModel> Handle(GetRentalsQuery request, CancellationToken cancellationToken)
        {
            var rental = await _dbContext.Rentals
                .AsNoTracking()
                .Include(r => r.Units)
                .FirstOrDefaultAsync(b => b.Id == request.RentalId, cancellationToken);

            if (rental == null)
            {
                throw new ValidationException("Rental not found");
            }

            return _mapper.Map<RentalViewModel>(rental);
        }
    }
}
