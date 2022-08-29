using System;
using System.Linq.Expressions;
using VacationRental.Core.Infrastructure.Specifications;

namespace VacationRental.Core.Domain.Bookings.Spec
{
    public class FilterByDate : Specification<BookingEntity>
    {
        private readonly DateTime _start;

        public FilterByDate(DateTime start)
        {
            _start = start;
        }

        public override Expression<Func<BookingEntity, bool>> ToExpression()
        {
            return x => x.Start == _start;
        }
    }
}
