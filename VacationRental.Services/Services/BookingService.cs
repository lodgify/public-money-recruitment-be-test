using System;
using VacationRental.Data.IRepository;
using VacationRental.Domain.Models;
using VacationRental.Services.IServices;
using VacationRental.Domain.Translator.Booking;

namespace VacationRental.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingHelper _bookingHelper;

        public BookingService(IBookingRepository bookingRepository, IBookingHelper bookingHelper)
        {
            _bookingRepository = bookingRepository;
            _bookingHelper = bookingHelper;
        }

        public ResourceIdViewModel Create(BookingBindingModel input)
        {
            if (input.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            else if (input.Start <= DateTime.Now)
                throw new ApplicationException("Booking date should be future date");

            bool canBook = _bookingHelper.CheckVacancy(input.RentalId, input.Start, input.StayPeriod);
            if (!canBook)
                throw new ApplicationException("Unable to book");

            var bookingViewModel = input.ViewModelTranslator();
            bookingViewModel = _bookingRepository.Add(bookingViewModel);

            return new ResourceIdViewModel { Id = bookingViewModel.Id };
        }

        public BookingViewModel Get(int id)
        {
            return _bookingRepository.GetById(id);
        }
    }
}
