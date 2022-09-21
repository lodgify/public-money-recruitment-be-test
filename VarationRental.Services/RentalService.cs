using AutoMapper;
using VacationRental.Data.Model;
using VacationRental.Data.Repositories.Abstractions;
using VacationRental.Services.Abstractions;
using VacationRental.Services.Dto;

namespace VacationRental.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IMapper _mapper;

    public RentalService(IRentalRepository rentalRepository, IMapper mapper)
    {
        _rentalRepository = rentalRepository;
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
}