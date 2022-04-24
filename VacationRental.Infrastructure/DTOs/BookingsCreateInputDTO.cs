using FluentValidation;
using Mapster;
using VacationRental.Domain.Bookings;

namespace VacationRental.Infrastructure.DTOs
{
    public class BookingsCreateInputDTO
    {
        public BookingsCreateInputDTO()
        {
        }

        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;

        public int Nights { get; set; }
    }

    public class BookingsCreateInputDTOMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<BookingsCreateInputDTO, Booking>()
                .Map(dest => dest.RentalId, src => src.RentalId)
                .Map(dest => dest.Nights, src => src.Nights)
                .Map(dest => dest.Start, src => src.Start)
                .Map(dest => dest.Unit, src => MapContext.Current.Parameters[nameof(Booking.Unit)]);           
        }
    }

    public class BookingsCreateInputDTOValidator : AbstractValidator<BookingsCreateInputDTO>
    {
        public BookingsCreateInputDTOValidator()
        {
            RuleFor(x => x.Nights).GreaterThan(0).WithMessage("Nights must be positive");
            RuleFor(x => x.Start).GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Start Date should be greater or equal to today's date");
        }
    }
}
