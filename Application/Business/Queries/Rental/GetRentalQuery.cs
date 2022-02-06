using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Rental.Responses;
using AutoMapper;
using Domain.DAL;
using MediatR;

namespace Application.Business.Queries.Rental
{
    public class GetRentalQuery : IRequest<RentalResponse>
    {
        public int RentalId { get; }
        
        public GetRentalQuery(int rentalId)
        {
            RentalId = rentalId;
        }
    }
    
    public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, RentalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.DAL.Models.Rental> _repository;

        public GetRentalQueryHandler(IRepository<Domain.DAL.Models.Rental> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<RentalResponse> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mapper.Map<RentalResponse>(_repository.Find(request.RentalId)));
        }
    }
}