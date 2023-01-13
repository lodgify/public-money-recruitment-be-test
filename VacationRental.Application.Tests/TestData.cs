using AutoMapper;
using VacationRental.Application.Mapping;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Tests
{
    public static class TestData
    {
        public static Rental CreateRentalForTest() => Rental.Create(1, 2);

        public static Booking CreateBookingForTest(int rentalId, DateTime start) => Booking.Create(rentalId, start, 2, 1);        

        public static IMapper CreateMapForTest()
        {
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });

            return mapperConfig.CreateMapper();
        }
    }
}
