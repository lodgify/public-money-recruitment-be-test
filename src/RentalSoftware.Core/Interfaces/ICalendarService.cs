using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Contracts.Response;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Interfaces
{
    public interface ICalendarService
    {
        Task<GetCalendarResponse> GetCalendar(GetCalendarRequest request);
    }
}
