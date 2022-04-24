using Mapster;
using VacationRental.Data;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.DTOs;
using VacationRental.Infrastructure.Services.Interfaces;

namespace VacationRental.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IEntityRepository<Booking> _bookingRepository;
        private readonly IEntityRepository<Rental> _rentalRepository;

        public BookingService(IEntityRepository<Booking> bookingRepository, IEntityRepository<Rental> rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public int CreateBooking(BookingsCreateInputDTO inputDTO)
        {
            try
            {
                var rental = _rentalRepository.GetEntityById(inputDTO.RentalId);

                var availableUnit = GetAvailableUnit(rental, inputDTO);

                var booking = inputDTO
                    .BuildAdapter()
                    .AddParameters(nameof(Booking.Unit), availableUnit)
                    .AdaptToType<Booking>();

                var result = _bookingRepository.Add(booking);

                return result.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Booking GetBooking(int id)
        {
            try
            {
                try
                {
                    return _bookingRepository.GetEntityById(id);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int GetAvailableUnit(Rental rental, BookingsCreateInputDTO input)
        {
            int availableUnit = 1;

            var bookings = _bookingRepository.FindEntities(x =>
                    x.RentalId == rental.Id && DoDatesOverlapse(x, input, rental.PreparationTime));

            if (bookings == null || !bookings.Any())
            {
                return availableUnit;
            }

            for (int i = availableUnit; i <= rental.Units; i++)
            {
                if (bookings.All(x => x.Unit != i))
                {
                    return i;
                }
            }

            throw new ApplicationException("Not Available");
        }

        private bool DoDatesOverlapse(Booking booking, BookingsCreateInputDTO input, int preparationTime)
        {
            var isStartDateInTheMiddle =
                booking.Start <= input.Start.Date &&
                booking.Start.AddDays(booking.Nights + preparationTime) > input.Start.Date;

            var isEndDateInTheMiddle =
                booking.Start < input.Start.AddDays(input.Nights + preparationTime) &&
                booking.Start.AddDays(booking.Nights) >= input.Start.AddDays(input.Nights);

            var isBookingInTheMiddle =
                booking.Start > input.Start &&
                booking.Start.AddDays(booking.Nights) < input.Start.AddDays(input.Nights);

            return isStartDateInTheMiddle || isEndDateInTheMiddle || isBookingInTheMiddle;
        }
    }
}
