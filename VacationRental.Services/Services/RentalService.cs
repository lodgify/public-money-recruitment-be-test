using System;
using VacationRental.Data.IRepository;
using VacationRental.Domain.Models;
using VacationRental.Domain.Translator.Rental;
using VacationRental.Services.IServices;

namespace VacationRental.Services.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingHelper _bookingHelper;

        public RentalService(IRentalRepository rentalRepository, IBookingHelper bookingHelper)
        {
            _rentalRepository = rentalRepository;
            _bookingHelper = bookingHelper;
        }

        public ResourceIdViewModel Create(RentalBindingModel model)
        {
            var rentalViewModel = model.ViewModelTranslator();

            rentalViewModel = _rentalRepository.Add(rentalViewModel);

            return new ResourceIdViewModel { Id = rentalViewModel.Id };
        }

        public RentalViewModel Get(int id)
        {
            return _rentalRepository.GetById(id);
        }

        public RentalViewModel Update(int id, RentalBindingModel rental)
        {
            var rentalViewModel = rental.ViewModelTranslator();
            rentalViewModel.Id = id;

            bool canChange = _bookingHelper.CheckCanChange(id, rental.Units, rental.PreparationTimeInDays);
            if (!canChange) 
                throw new ApplicationException("booking not available");

            return _rentalRepository.Update(rentalViewModel);
        }
    }
}
