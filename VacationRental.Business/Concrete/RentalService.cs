using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Business.Abstract;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;

namespace VacationRental.Business.Concrete
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository rentalRepository;

        public RentalService(IRentalRepository rentalRepository)
        {
            this.rentalRepository = rentalRepository;
        }

        public Rental Create(Rental rental)
        {
            return rentalRepository.Create(rental);
        }

        public Rental GetById(int id)
        {
            return rentalRepository.GetById(id);
        }

        public Rental Update(Rental rental, RentalDto dto)
        {
            return rentalRepository.Update(rental, dto);
        }
    }
}
