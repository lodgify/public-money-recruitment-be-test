using System;
using VacationRental.Domain.Interfaces;
using VacationRental.Rental.Entities.Rentals.Events;

namespace VacationRental.Rental.Domain
{
    public partial class Rental : IAggregateRoot
    {

        public Rental(Rental rental) : this(rental.Units, rental.PreparationTimeInDays, rental.Guid) { }

        public Rental(int unit, int preparationTime, Guid? guid)
        {
            this.Guid = guid ?? this.Guid;
            this.Update(unit, preparationTime, false);
        }

        public override object HandleClone()
        {
            return new Rental(this);
        }

        public void Update(int unit, int preparationDays, bool addEvent = true)
        {
            this.Units = unit;
            this.PreparationTimeInDays = preparationDays;
            if (addEvent)
            {
                AddEvent(new OnRentalUpdatedDomainEvent(this));
            }
        }

    }
}