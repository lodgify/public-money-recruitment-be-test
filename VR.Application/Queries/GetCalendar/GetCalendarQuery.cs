using MediatR;
using System;

namespace VR.Application.Queries.GetCalendar
{
    public class GetCalendarQuery : IRequest<GetCalendarResponse>
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
