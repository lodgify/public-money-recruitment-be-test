using System.Collections.Generic;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Infrastructure.Commons
{
    public static class CommonHelper
    {
        public static ResourceIdViewModel CreateResourceId(this IDictionary<int, BookingViewModel> bookings)
            => new ResourceIdViewModel { Id = bookings.Keys.Count + 1 };

        public static ResourceIdViewModel CreateResourceId(this IDictionary<int, RentalViewModel> bookings)
            => new ResourceIdViewModel { Id = bookings.Keys.Count + 1 };

        public static ResourceIdViewModel GetResourceId(this BookingViewModel booking)
            => new ResourceIdViewModel { Id = booking.Id };

        public static ResourceIdViewModel GetResourceId(this RentalViewModel rental)
            => new ResourceIdViewModel { Id = rental.Id };
    }
}
