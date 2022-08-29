using System;

namespace VacationRental.Core
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public TId Id { get; set; }

        public virtual string GetEntityName()
        {
            return GetType().Name;
        }

        public virtual bool IsTransientRecord()
        {
            return Id.Equals(default(TId));
        }

        #region Equals

        bool IEquatable<BaseEntity<TId>>.Equals(BaseEntity<TId> other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity<TId>);
        }

        protected virtual bool Equals(BaseEntity<TId> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (HasSameNonDefaultIds(other))
            {
                var otherType = other.GetType();
                var thisType = GetType();
                return thisType.Equals(otherType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            // It's possible for two objects to return the same hash code based on
            // identically valued properties, even if they're of two different types,
            // so we include the object's type in the hash calculation
            var hashCode = GetType().GetHashCode();

            if (IsTransientRecord())
            {
                return hashCode;
            }
            else
            {
                unchecked
                {
                    return (hashCode * 31) ^ Id.GetHashCode();
                }
            }
        }

        public static bool operator ==(BaseEntity<TId> x, BaseEntity<TId> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity<TId> x, BaseEntity<TId> y)
        {
            return !Equals(x, y);
        }

        private bool HasSameNonDefaultIds(BaseEntity<TId> other)
        {
            return !IsTransientRecord() && !other.IsTransientRecord() && Id.Equals(other.Id);
        }

        #endregion
    }
}
