using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Common.Models;
using VacationRental.Service.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IBookingService _bookService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalService rentalService, IBookingService bookingService, IMapper mapper)
        {
            _rentalService = rentalService;
            _bookService = bookingService;
            _mapper = mapper;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, RentalBindingModel model)
        {
            var rental = _mapper.Map<RentalViewModel>(model);
            if (!_rentalService.UpdateRental(rentalId, ref rental))
            {
                throw new ApplicationException($"Not able to update rental {rentalId}");
            }
            return rental;
        }
        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalService.Get(rentalId);
            if (rental is null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            return _rentalService.CreateRental(_mapper.Map<RentalViewModel>(model));
        }
    }
}
