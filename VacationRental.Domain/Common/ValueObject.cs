using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Domain.Common
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetAttributesToIncludeInEqualityCheck().SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public override int GetHashCode()
        {
            return GetAttributesToIncludeInEqualityCheck()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
