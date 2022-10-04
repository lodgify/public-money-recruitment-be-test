using AutoMapper;
using VacationRental.Application.Bookings.Commands.CreateBooking;
using VacationRental.Application.Bookings.Models;
using VacationRental.Application.Rentals.Commands.CreateRental;
using VacationRental.Application.Rentals.Models;
using VacationRental.Domain.Entities;

namespace VacationRental.Application
{
    public class VacationRentalMappings : Profile
    {
        public VacationRentalMappings()
        {
            CreateMap<Booking, ResourceIdViewModel>();
            CreateMap<CreateBookingCommand, Booking>()
                .ForMember(x => x.Start, opts => opts.MapFrom(x => x.Start.Date))
                .ForMember(x => x.End, opts => opts.MapFrom(x => x.Start.AddDays(x.Nights).Date));
            CreateMap<Booking, BookingViewModel>()
                .ForMember(x => x.RentalId, opts => opts.MapFrom(x => x.Unit.RentalId))
                .ForMember(x => x.Start, opts => opts.MapFrom(x => x.Start))
                .ForMember(x => x.Nights, opts => opts.MapFrom(x => (x.End - x.Start).Days));

            CreateMap<Rental, ResourceIdViewModel>();
            CreateMap<CreateRentalCommand, Rental>();
            CreateMap<Rental, RentalViewModel>()
                .ForMember(x => x.Units, opts => opts.MapFrom(x => x.Units.Count));


            CreateMap<Unit, ResourceIdViewModel>();
        }
    }
}
