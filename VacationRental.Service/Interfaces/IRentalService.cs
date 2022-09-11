using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Common.Models;
using VacationRental.Data.Entities;
using VacationRental.Repository.Interfaces;

namespace VacationRental.Service.Interfaces
{
    public interface IRentalService : IBaseService<IBaseRepository<Rental>, RentalViewModel, Rental>
    {
    }
}
