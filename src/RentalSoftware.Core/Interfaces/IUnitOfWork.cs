using System.Threading.Tasks;

namespace RentalSoftware.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IBookingRepository BookingRepository { get; }
        IRentalRepository RentalRepository { get; }
        Task<int> Complete();
    }
}
