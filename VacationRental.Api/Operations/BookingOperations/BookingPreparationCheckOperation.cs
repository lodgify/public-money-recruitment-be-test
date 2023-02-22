using Models.DataModels;
using System.ComponentModel.DataAnnotations;
using Models.Models;
using VacationRental.Api.Constants;
using Repository.Repository;

namespace VacationRental.Api.Operations.BookingOperations;

public sealed class BookingPreparationCheckOperation : IBookingPreparationCheckOperation
{
    private readonly IBookingRepository _bookingRepository;

    public BookingPreparationCheckOperation(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public Task<IEnumerable<BookingDto>> ExecuteAsync(int rentalId, int preparationTimeInDays, DateTime orderStartDate)
    {
        if (rentalId <= 0)
            throw new ValidationException(ExceptionMessageConstants.RentalIdValidationError);
        if (preparationTimeInDays <= 0)
            throw new ValidationException(ExceptionMessageConstants.PreparationTimeUpdateError);

        return DoExecuteAsync(rentalId, preparationTimeInDays, orderStartDate);
    }

    private async Task<IEnumerable<BookingDto>> DoExecuteAsync(
        int rentalId,
        int preparationTimeInDays,
        DateTime orderStartDate)
    {
        var bookings = await _bookingRepository.GetAll(rentalId, orderStartDate);

        if (!bookings.Any())
            return bookings;

        var allBookingsList = bookings.Select(_ => new BookingProcessingModel
        {
            Unit = _.Unit,
            Start = _.Start,
            End = _.Start.AddDays(_.Nights + _.PreparationTimeInDays)
        })
            .GroupBy(_ => _.Unit)
            .Select(_ => _.ToArray())
            .ToArray();

        for (var i = 0; i < allBookingsList.Length - 1; i++)
        {
            if (allBookingsList[i].Length <= 1)
                continue;

            for (var j = 0; j < allBookingsList[i].Length - 1; j++)
            {
                var diffrenceDays = (int)(allBookingsList[i][j].End - allBookingsList[i][j + 1].Start).TotalDays;
                if (diffrenceDays < preparationTimeInDays)
                    throw new ValidationException(ExceptionMessageConstants.PreparationTimeUpdateError);
            }
        }

        return bookings;
    }
}
