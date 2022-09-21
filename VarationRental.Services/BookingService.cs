using AutoMapper;
using VacationRental.Data.Model;
using VacationRental.Data.Model.Enums;
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

        for (var i = 0; i < newBooking.Nights + rental.PreparationTimeInDays; i++)
        {
            var count =
                _bookingRepository
                    .GetAll()
                    .Count(booking => booking.RentalId == newBooking.RentalId
                        && (booking.Start <= newBooking.Start.Date && booking.Start.AddDays(booking.Nights) > newBooking.Start.Date)
                        || (booking.Start < newBooking.Start.AddDays(newBooking.Nights) && booking.Start.AddDays(booking.Nights) >= newBooking.Start.AddDays(newBooking.Nights))
                        || (booking.Start > newBooking.Start && booking.Start.AddDays(booking.Nights) < newBooking.Start.AddDays(newBooking.Nights)));

            if (rental is null || count >= rental.Units || rental.Units < newBooking.Unit)
                throw new ApplicationException("Rental is not available");
        }

        var bookingData = _mapper.Map<BookingDto, Booking>(newBooking);
        bookingData.Type = BookingType.Booking;
        var newId = _bookingRepository.Add(bookingData).Id;

        if (rental.PreparationTimeInDays > 0)
        {
            var serviceData = new Booking
            {
                Nights = rental.PreparationTimeInDays,
                RentalId = rental.Id,
                Unit = bookingData.Unit,
                Start = bookingData.Start.AddDays(bookingData.Nights),
                Type = BookingType.Service
            };

            _bookingRepository.Add(serviceData);
        }

        return newId;
    }
}