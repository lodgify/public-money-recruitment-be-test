using VacationRental.Domain.Models;
using VacationRental.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace VacationRental.Domain.Services
{
	/// <summary>
	/// This class will be registered as a service and will handle the domain's Rental logic
	/// </summary>
	public class RentalService : IRentalService
	{
		private readonly GIRepository<Rental> repository;

        /// Rental Service constructor.
        private readonly GIRepository<Rental> repo;

        //Injected the repository for the Rental table
        public RentalService(GIRepository<Rental> repository)
        {
            this.repo = repository;
        }
        public async Task<Rental> AddAsync(Rental rentalToAdd)
        {
            try
            {
                await repo.Add(rentalToAdd);
                await repo.Save();
                return rentalToAdd;
            }
            catch (Exception ex)
            {
                return null;
                // throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
            }
            //throw new NotImplementedException();
        }

        public Rental GetById(int rentalId)
        {
            try
            {
                var rental = repo.Query.Where(x => x.Id == rentalId).FirstOrDefault();
                return rental;
            }
            catch (Exception ex)
            {
                return null;
            }
            //throw new NotImplementedException();
        }

        public Rental Update(Rental rental)
        {
            try
            {
                repo.Update(rental);
                repo.Save();
                return rental;
            }
            catch (Exception ex)
            {
                return null;
            }
            //throw new NotImplementedException();
        }
    }
}
