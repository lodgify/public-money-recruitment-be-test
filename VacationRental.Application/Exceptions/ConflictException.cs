using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Primitives;

namespace VacationRental.Application.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(DomainError error) : base(error)
        {
        }
    }
}
