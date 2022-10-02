using System.Runtime.Serialization;
using VacationRental.Domain.Extensions.Common;
using VacationRental.Domain.VacationRental.Extensions.Enum;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Interfaces.Repositories;
using VacationRental.Domain.VacationRental.Models;
using VacationRental.Domain.VacationRental.Utils;

namespace VacationRental.Domain.VacationRental.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalsService _rentalsService;

        public BookingService(IBookingRepository paramBookingRepository, IRentalsService paramRentalsService)
        {
            _bookingRepository = paramBookingRepository;
            _rentalsService = paramRentalsService;
        }

        public async Task<BookingViewModel> Get(int bookingId)
        {
            try
            {
                return await _bookingRepository.Get(bookingId);
            }
            catch(Exception)
            {
                throw new NotFoundException(EnumExceptions.BookingNotFound.GetAttributeOfType<EnumMemberAttribute>().Value);
            }
        }

        public async Task<List<BookingViewModel>> Get()
        {
            try
            {
                return await _bookingRepository.Get();
            }
            catch (Exception)
            {
                throw new NotFoundException(EnumExceptions.BookingNotFound.GetAttributeOfType<EnumMemberAttribute>().Value);
            }
        }

        public async Task<List<BookingViewModel>> GetByRentalId(int rentalId)
        {
            try
            {
                return await _bookingRepository.GetByRentalId(rentalId);
            }
            catch (Exception)
            {
                throw new NotFoundException(EnumExceptions.BookingNotFound.GetAttributeOfType<EnumMemberAttribute>().Value);
            }
        }

        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ConflictException(EnumExceptions.NightsConflict.GetAttributeOfType<EnumMemberAttribute>().Value);

            var rentals = await _rentalsService.Get(model.RentalId);

            var bookings = await _bookingRepository.GetByRentalId(model.RentalId);

            int unit = 0;

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                
                foreach (var booking in bookings)
                {
                    if ((booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights + rentals.PreparationTimeInDays) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights + rentals.PreparationTimeInDays) && booking.Start.AddDays(booking.Nights + rentals.PreparationTimeInDays) >= model.Start.AddDays(model.Nights + rentals.PreparationTimeInDays))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights + rentals.PreparationTimeInDays) < model.Start.AddDays(model.Nights + rentals.PreparationTimeInDays)))
                    {
                        count++;
                        unit = booking.Unit + 1;
                    }
                }
                if (count >= rentals.Units)
                    throw new ConflictException(EnumExceptions.AvailableConflict.GetAttributeOfType<EnumMemberAttribute>().Value);

                if (unit == 0) unit = 1;
                    
            }

            int? lastId = await _bookingRepository.GetLastId();
            lastId = lastId is null ? 0 : lastId;

            var NewBooking =  new BookingViewModel
            {
                Id = (int)lastId + 1,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                End = model.Start.AddDays(model.Nights + rentals.PreparationTimeInDays),
                Unit = unit
            };          

            return await _bookingRepository.Post(NewBooking);
        }
    }
}
