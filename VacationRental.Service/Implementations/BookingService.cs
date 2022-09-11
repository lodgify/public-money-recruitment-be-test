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
    public class BookingService : BaseService<IBaseRepository<Booking>, BookingViewModel, Booking>, IBookingService
    {
        public BookingService(IMapper mapper, IBookingRepository repository) : base(mapper, repository)
        {


        }
    }
}
