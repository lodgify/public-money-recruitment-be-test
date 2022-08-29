using System;
using System.Linq.Expressions;
using VacationRental.Core.Infrastructure.Specifications;

namespace VacationRental.Core.Domain.Bookings.Spec
{
    public class FilterByRentalId : Specification<BookingEntity>
    {
        private readonly int _rentalId;

        public FilterByRentalId(int rentalId)
        {
            _rentalId = rentalId;
        }

        public override Expression<Func<BookingEntity, bool>> ToExpression()
        {
            return x => x.RentalId == _rentalId;
        }
    }
}