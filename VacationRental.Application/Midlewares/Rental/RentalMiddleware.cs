﻿using System;
using System.Threading.Tasks;
using VacationRental.Application.Dtos;
using VacationRental.Domain.PreparationTimes;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Application.Midlewares.Rental
{
	public class RentalMiddleware : IRentalMiddleware
	{
		private readonly IRentalRepository _rentalRepository;

		public RentalMiddleware(IRentalRepository rentalRepository)
		{
			this._rentalRepository = rentalRepository;
		}

		public async Task<int> AddRentalWithTimePeriod(RentalDto input)
		{
			if (input.Units <= 0) 
			{
				throw new Exception("Unities must be higher than zero");
			}

			var rental = new Domain.Rentals.Rental(input.Units);
			var timePeriods = new PreparationTime(input.Units, rental);
			rental.PreparationTimes.Add(timePeriods);
			var rentalId = await this._rentalRepository.AddRental(rental);
			return rentalId;
		}
	}
}
