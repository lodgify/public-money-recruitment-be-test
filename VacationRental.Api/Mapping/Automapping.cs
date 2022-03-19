using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.WebAPI.DTOs;

namespace VacationRental.WebAPI.Mapping
{
	/// <summary>
	/// This class defines the mapping between DTOs and models.
	/// <para/>
	/// Custom mapping can also be added by using .ConvertUsing<CustomConverter>() where CustomConverter would be a class that implements ITypeConverter;
	/// </summary>
	public class Automapping : Profile
	{
		public Automapping()
		{
			//Rental
			CreateMap<RentalRequestDTO, Rental>();
			CreateMap<Rental, ResourceIdViewModel>();

			//Booking
			CreateMap<BookingBindingModel, Booking>();
			CreateMap<Booking, ResourceIdViewModel>();
			CreateMap<Booking, BookingViewModel>();

			//Calendar
			CreateMap<CalendarRequestDTO, CalendarRequest>();
			CreateMap<Calendar, CalendarResponse>();
			CreateMap<CalendarDate, CalendarDateResponse>();
			CreateMap<CalendarBooking, CalendarBookingResponse>();
			CreateMap<CalendarBooking, PreparationTimeResponse>();
		}
	}
}
