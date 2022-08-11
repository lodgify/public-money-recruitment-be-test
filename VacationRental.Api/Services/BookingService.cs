using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<BookingBindingModel> _validator;

        public BookingService(
            IBookingRepository bookingRepository,
            IMapper mapper,
            IValidator<BookingBindingModel> validator
        )
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public BookingViewModel GetBooking(int id)
        {
            var booking = _bookingRepository.GetBooking(id);

            if (booking is null)
                throw new ApplicationException("Booking not found");

            return booking;
        }

        public async Task<ResourceIdViewModel> CreateAsync(BookingBindingModel model)
        {
            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                throw new ApplicationException(validationResult.Errors.First().ErrorMessage);

            var newBooking = _mapper.Map<BookingViewModel>(
                model,
                opt => opt.Items["Id"] = _bookingRepository.BookingCount() + 1
            );

            var bookingId = _bookingRepository.CreateBooking(newBooking);

            return new ResourceIdViewModel { Id = bookingId };
        }
    }
}
