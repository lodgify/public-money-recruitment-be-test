using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.Business.Abstract
{
    public interface IRentalService
    {
        Rental Create(Rental rental);
        Rental GetById(int id);
        Rental Update(Rental rental, RentalDto dto);
    }
}
