using AutoMapper;
using MediatR;
using System;
using VacationRental.Domain;
using Xunit;

namespace VacationRental.Application.UnitTests
{
    public class RequestHandlerTestBase<THandler, TRequest, TResponse>
        where THandler : IRequestHandler<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>, new()
    {
        private THandler _handler;
        private VacationRentalDbContext DbContext { get; }
        private IMapper Mapper { get; }

        protected RequestHandlerTestBase(VacationRentalDbContext dbContext)
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<VacationRentalMappings>()).CreateMapper();
            DbContext = dbContext;
            Handler = CreateRequestHandler();
        }

        protected IRequestHandler<TRequest, TResponse> Handler { get; }

        protected virtual THandler CreateRequestHandler()
        {
            return (THandler)Activator.CreateInstance(typeof(THandler), DbContext, Mapper);
        }
    }
}
