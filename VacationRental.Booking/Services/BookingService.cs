using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Booking.Domain.EventHandlers;
using VacationRental.Booking.Domain.Interfaces;
using VacationRental.Booking.Entities.Rentals.Events;
using VacationRental.Domain.Interfaces;
using VacationRental.Domain.Services;

namespace VacationRental.Booking.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBookingModule(this IServiceCollection services)
        {
            //services.AddMediatR(typeof(OnRentalUpdatedDomainEvent).Assembly);
            services.AddMediatR(typeof(RentalEventHandler).Assembly);

            return services.AddScoped<BookingService>();
        }
    }
    public class BookingService : BaseService
    {
        public BookingService(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {

        }

        public async Task<Domain.Calendar> GetCalendarAsync(IBooking request)
        {
            var rentals = UnitOfWork.AsyncRepository<Rental.Domain.Rental>();
            Rental.Domain.Rental rental = await rentals.GetAsync(request.RentalId);

            if (rental == null)
            {
                throw new ApplicationException("ID Not found");
            }

            var bookings = UnitOfWork.AsyncRepository<Domain.Booking>();
            Domain.Calendar calendar = Domain.Calendar.Create(request.RentalId, new List<Domain.CalendarDay>());

            var requestedBookings = await bookings.ListAsync((x) =>
            {
                IBooking booking = (IBooking)x;
                DateTime bookingEndDate = booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays);
                DateTime requestEndDate = request.Start.AddDays(request.Nights);
                return booking.RentalId == request.RentalId
                && (
                    (booking.Start > request.Start && booking.Start < requestEndDate)
                    || (bookingEndDate > request.Start && bookingEndDate < requestEndDate)
                    );
            });
            for (int Day = 0; Day < request.Nights; Day++)
            {
                Domain.CalendarDay day = new Domain.CalendarDay(request.Start.AddDays(Day), new List<Domain.CalendarBooking>(), new List<Domain.CalendarUnit>());
                foreach (Domain.Booking dayBooking in requestedBookings.Where(x => x.Start <= day.Date && x.Start.AddDays(x.Nights + rental.PreparationTimeInDays) >= day.Date))
                {
                    if (dayBooking.Start.AddDays(dayBooking.Nights) > day.Date)
                    {
                        day.Bookings.Add(new Domain.CalendarBooking
                        {
                            Id = dayBooking.Id,
                            Unit = dayBooking.Unit
                        });
                    }
                    else
                    {
                        day.PreparationTimes.Add(new Domain.CalendarUnit { Unit = dayBooking.Unit });
                    }
                }
                calendar.Dates.Add(day);
            }
            return calendar;
        }
        public async Task<IBookingId> AddNewAsync(IBooking addRequest)
        {
            var rentals = UnitOfWork.AsyncRepository<Rental.Domain.Rental>();

            Rental.Domain.Rental rental = await rentals.GetAsync(addRequest.RentalId);
            if (rental == null)
            {
                throw new ApplicationException("Rental not found");
            }

            var bookings = UnitOfWork.AsyncRepository<Domain.Booking>();
            var currentBookings = (await bookings.ListAsync(x => ((IBooking)x).RentalId == rental.Id)).ToList<IBooking>();
            Domain.Booking booking = Domain.Booking.Create(addRequest, currentBookings, rental);

            await Mediator.Publish(new OnBookingCreatedDomainEvent(booking, rental));
            var result = await bookings.AddAsync(booking);
            await UnitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<Domain.Booking> SearchAsync(IBookingId request)
        {
            var bookings = UnitOfWork.AsyncRepository<Domain.Booking>();
            var rental = await bookings.GetAsync(request.Id);
            return rental;
        }

        internal async Task<IBooking> UpdateAsync(IBooking request)
        {
            var bookings = UnitOfWork.AsyncRepository<Domain.Booking>();
            Domain.Booking rental = await bookings.GetAsync(request.Id);

            rental.Update(request);
            try
            {
                Task.WaitAll(rental.Events.Select(x => Mediator.Publish(x)).ToArray());
                await bookings.UpdateAsync(rental);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (ApplicationException ex)
            {
                await UnitOfWork.RollBack<Domain.Booking>();
                throw new ApplicationException("Rolled Back update", ex);
            }
            return rental;
        }
    }
}