using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/rentals")]
	[ApiController]
	public class RentalsController : ControllerBase
	{
		private readonly IDictionary<int, RentalViewModel> _rentals;

		public RentalsController(IDictionary<int, RentalViewModel> rentals)
		{
			_rentals = rentals;
		}

		[HttpGet]
		[Route("{rentalId:int}")]
		public RentalViewModel Get(int rentalId)
		{
			bool containsKey = _rentals.ContainsKey(rentalId);
			if (!containsKey)
			{
				string errMsg = "Rental not found";
				throw new ApplicationException(errMsg);
			}

			RentalViewModel result = _rentals[rentalId];
			return result;
		}

		[HttpPost]
		public ResourceIdViewModel Post(RentalBindingModel model)
		{
			int newId = _rentals.Keys.Count + 1;
			ResourceIdViewModel result = ResourceIdViewModel.Create(newId);

			RentalViewModel rental = RentalViewModel.Create(result.Id, model.Units, model.PreparationTimeInDays);
			_rentals.Add(result.Id, rental);

			return result;
		}
	}
}
