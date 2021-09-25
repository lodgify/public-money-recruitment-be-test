using System.Collections.Generic;
using VacationRental.Domain.Common;

namespace VacationRental.Domain.Values
{
    public sealed class RentalId : ValueObject
    {
        public static RentalId Empty { get; } = new RentalId(int.MinValue);

        public RentalId(int id)
        {
            Id = id;
        }

        public int Id { get; }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck() => new List<object>{Id};

        public override string ToString() => Id.ToString();

        public static explicit operator int(RentalId @this) => @this.Id;
    }
}
