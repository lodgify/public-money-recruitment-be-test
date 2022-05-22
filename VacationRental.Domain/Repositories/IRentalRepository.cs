using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Repositories
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Rental Update(Rental rental, RentalDto dto);
    }
}
