using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Data.IRepository;
using VacationRental.Data.Store;

namespace VacationRental.Data.Repository
{
    public class BaseRepository
    {
        protected readonly IDataContext _context;

        public BaseRepository(IDataContext context)
        {
            _context = context;
        }

       
    }
}
