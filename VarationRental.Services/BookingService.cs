using AutoMapper;
using VacationRental.Data.Model;
using VacationRental.Data.Repositories.Abstractions;
using VacationRental.Services.Abstractions;
using VacationRental.Services.Dto;

namespace VacationRental.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository bookingRepository, IRentalRepository rentalRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _rentalRepository = rentalRepository;
        _mapper = mapper;
    }

    public BookingDto Get(int id)
    {
        return _mapper.Map<Booking, BookingDto>(_bookingRepository.Get(id));
    }

    public int Create(BookingDto newBooking)
    {
        var rental = _rentalRepository.Get(newBooking.RentalId);

        for (var i = 0; i < newBooking.Nights; i++)
        {
            var count =
                _bookingRepository
                    .GetAll()
                    .Count(booking => booking.RentalId == newBooking.RentalId
                        && (booking.Start <= newBooking.Start.Date && booking.Start.AddDays(booking.Nights) > newBooking.Start.Date)
                        || (booking.Start < newBooking.Start.AddDays(newBooking.Nights) && booking.Start.AddDays(booking.Nights) >= newBooking.Start.AddDays(newBooking.Nights))
                        || (booking.Start > newBooking.Start && booking.Start.AddDays(booking.Nights) < newBooking.Start.AddDays(newBooking.Nights)));

            if (rental is null || count >= rental.Units)
                throw new ApplicationException("Rental is not available");
        }

        return _bookingRepository.Add(_mapper.Map<BookingDto, Booking>(newBooking)).Id;
    }
}