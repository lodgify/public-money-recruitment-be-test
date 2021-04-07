namespace VacationalRental.Domain.Enums
{
    public enum InsertUpdateNewRentalStatus
    {
        InsertUpdateDbNoRowsAffected = 1,
        OK,
        RentalNotExists,
        UnitsQuantityBookedAlready,
        DatesOverlapping
    }
}
