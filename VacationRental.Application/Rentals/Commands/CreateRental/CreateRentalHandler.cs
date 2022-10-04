using AutoMapper;
using MediatR;
using VacationRental.Domain;
using VacationRental.Domain.Entities;
using Unit = VacationRental.Domain.Entities.Unit;

namespace VacationRental.Application.Rentals.Commands.CreateRental
{
    public class CreateRentalHandler : IRequestHandler<CreateRentalCommand, ResourceIdViewModel>
    {
        private readonly VacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateRentalHandler(VacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = _mapper.Map<Rental>(request);
            rental.Units = Enumerable.Range(0, request.Units).Select(x => new Unit()).ToList();

            await _dbContext.Rentals.AddAsync(rental, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ResourceIdViewModel>(rental);
        }
    }
}
