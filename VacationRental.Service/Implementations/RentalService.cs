using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Common.Models;
using VacationRental.Data.Entities;
using VacationRental.Repository.Interfaces;
using VacationRental.Service.Interfaces;

namespace VacationRental.Service.Implementations
{
    public class RentalService : BaseService<IBaseRepository<Rental>, RentalViewModel, Rental>, IRentalService
    {
        public RentalService(IMapper mapper, IRentalRepository repository) : base(mapper, repository)
        {


        }
    }
}
