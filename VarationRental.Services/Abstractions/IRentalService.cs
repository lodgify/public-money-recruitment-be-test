using VacationRental.Services.Dto;

namespace VacationRental.Services.Abstractions;

public interface IRentalService
{
    public RentalDto Get(int id);
    public int Create(RentalDto newRental);
}
