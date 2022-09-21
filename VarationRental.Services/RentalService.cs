using AutoMapper;
using VacationRental.Data.Model;
using VacationRental.Data.Model.Enums;
using VacationRental.Data.Repositories.Abstractions;
using VacationRental.Services.Abstractions;
using VacationRental.Services.Dto;

namespace VacationRental.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public RentalService(IRentalRepository rentalRepository,IBookingRepository bookingRepository, IMapper mapper)
    {
        _rentalRepository = rentalRepository;
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public RentalDto Get(int id)
    {
        return _mapper.Map<Rental, RentalDto>(_rentalRepository.Get(id));
    }

    public int Create(RentalDto newRental)
    {
        return _rentalRepository.Add(_mapper.Map<RentalDto, Rental>(newRental)).Id;
    }

    public int Update(RentalDto rental)
    {
        var serviceBookings = _bookingRepository
            .GetAll()
            .Where(booking => booking.Type == BookingType.Service)
            .ToList();

        foreach (var booking in serviceBookings)
        {
            if(_bookingRepository.GetAll()
                .Any(existingBooking => existingBooking.RentalId == rental.Id &&
                     existingBooking.Id != booking.Id && 
                     existingBooking.Start < booking.Start.AddDays(rental.PreparationTimeInDays) && 
                     booking.Start < existingBooking.Start.AddDays(existingBooking.Nights)))
                throw new ApplicationException("Rental failed to update due to booking overlapping");
        }

        if(rental.PreparationTimeInDays >0)
            foreach (var booking in serviceBookings)
            {
                booking.Nights = rental.PreparationTimeInDays;
                _bookingRepository.Update(booking);
            }
        else
            for (var i = 0; i < serviceBookings.Count(); i++)
            {
                _bookingRepository.Delete(serviceBookings[i]);
            }

        return _rentalRepository.Update(_mapper.Map<RentalDto, Rental>(rental)).Id;
    }
}