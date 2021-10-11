using MediatR;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Services
{
    public class BaseService
    {
        public BaseService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            Mediator = mediator;
            UnitOfWork = unitOfWork;
        }

        protected IMediator Mediator { get; }
        protected internal IUnitOfWork UnitOfWork { get; set; }
    }
}