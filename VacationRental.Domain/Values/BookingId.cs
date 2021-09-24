using System.Collections.Generic;
using VacationRental.Domain.Common;

namespace VacationRental.Domain.Values
{
    public sealed class BookingId : ValueObject<BookingId>
    {

        public static BookingId Empty { get; } = new BookingId(int.MinValue);

        public BookingId(int id)
        {
            Id = id;
        }

        public int Id { get; }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck() => new List<object>{Id};


        public override string ToString() => Id.ToString();

        public static explicit operator int(BookingId @this) => @this.Id;

    }
}
