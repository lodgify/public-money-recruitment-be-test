using AutoMapper;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Services.Dto;

namespace VacationRental.Api.Models.Mapping;

public class ViewModelMappingProfile : Profile
{
    public ViewModelMappingProfile()
    {
        CreateMap<RentalBindingModel, RentalDto>();
        CreateMap<BookingBindingModel, BookingDto>();

        CreateMap<CalendarFilterModel, CalendarFilterDto>();
        CreateMap<BookingDto, BookingViewModel>();
        CreateMap<RentalDto, RentalViewModel>();
    }
}
