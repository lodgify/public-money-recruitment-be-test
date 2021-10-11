using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using VacationRental.Domain.Interfaces;
using VacationRental.Domain.Services;
using VacationRental.Rental.Domain.Interfaces;
using VacationRental.Rental.DTOs;
using VacationRental.Rental.Entities.Rentals.Events;

namespace VacationRental.Rental.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddRentalModule(this IServiceCollection services)
        {
            return services.AddScoped<RentalService>();
        }
    }
    public class RentalService : BaseService
    {
        public RentalService(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {

        }

        public async Task<IRentalId> AddNewAsync(AddRentalRequest model)
        {
            // You can you some mapping tools as such as AutoMapper
            var Rental = new Domain.Rental(model.Units, model.PreparationTimeInDays, null);

            var repository = UnitOfWork.AsyncRepository<Domain.Rental>();
            var result = await repository.AddAsync(Rental);
            await UnitOfWork.SaveChangesAsync();

            var response = new AddRentalResponse()
            {
                Id = result.Id
            };

            return response;
        }

        public async Task<Domain.Rental> SearchAsync(IRentalId request)
        {
            var repository = UnitOfWork.AsyncRepository<Domain.Rental>();
            var rental = await repository.GetAsync(request.Id);
            return rental;
        }

        internal async Task<IRental> UpdateAsync(IRental request)
        {
            var repository = UnitOfWork.AsyncRepository<Domain.Rental>();
            Domain.Rental rental = await repository.GetAsync(request.Id);

            rental.Update(request.Units, request.PreparationTimeInDays);
            try
            {
                foreach (var pendingEvent in rental.Events)
                {

                    await Mediator.Publish((OnRentalUpdatedDomainEvent)pendingEvent);
                }

                //Task.WaitAll(rental.Events.Select(x => ).ToArray());
                await repository.UpdateAsync(rental);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await UnitOfWork.RollBack<Domain.Rental>();
                throw new ApplicationException("Rolled Back update", ex);
            }

            return rental;
        }
    }
}