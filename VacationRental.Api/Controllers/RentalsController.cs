using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Extensions;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public RentalsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("Preparation Time must be positive");

            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }

        private ValueTask<List<int>> ProccessChunk(
            IEnumerable<KeyValuePair<int, BookingViewModel>> chunk,
            int preparationTimeInDays,
            int units,
            int rentalId,
            ConcurrentDictionary<DateOnly, int> virtualCalendar,
            List<BookingViewModel> bookingsTemporaryStorage)
        {
            var keysToRemove = new List<int>();

            foreach (var booking in chunk)
            {
                if (booking.Value.IsPreparation)
                    keysToRemove.Add(booking.Key);
                else
                {
                    var nightsIncluingPrepration = booking.Value.Nights + preparationTimeInDays;

                    booking.Value.Start.CreateDateOnlyRangeUntil(nightsIncluingPrepration).ToList().ForEach(day =>
                    {
                        virtualCalendar.AddOrUpdate(day, 1, (key, value) => value + 1);
                        if (!virtualCalendar.TryGetValue(day, out var value) || value > units)
                            throw new ApplicationException("Not available");
                    });

                    var preparationTimeKey = - 1;
                    bookingsTemporaryStorage.Add(new BookingViewModel
                    {
                        Id = preparationTimeKey,
                        RentalId = rentalId,
                        Start = booking.Value.Start.AddDays(booking.Value.Nights),
                        Nights = preparationTimeInDays,
                        IsPreparation = true
                    });
                }
            }
            return ValueTask.FromResult(keysToRemove);
        }

        [HttpPut("{rentalId:int}")]
        public async ValueTask<ResourceIdViewModel> Put([FromRoute] int rentalId, RentalBindingModel model)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var rental = _rentals[rentalId];
            var rentalCopy = new RentalViewModel
            {
                Id = rental.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            if(model.PreparationTimeInDays != rental.PreparationTimeInDays)
            {
                var bookings = _bookings.Where(x => x.Value.RentalId == rentalId).OrderBy(x => x.Value.Start).ToList();
                var bookingsTemporaryStorage = new List<BookingViewModel>();
                var virtualCalendar = new ConcurrentDictionary<DateOnly, int>();

                var chunks = bookings.Chunk(Math.Max((int)Math.Floor(bookings.Count / 4D), 1));
                var keysToRemove = await Task.WhenAll(chunks.Select(async chunk => await ProccessChunk(
                        chunk,
                        rentalCopy.PreparationTimeInDays,
                        rentalCopy.Units,
                        rentalCopy.Id,
                        virtualCalendar,
                        bookingsTemporaryStorage)));

                // we are reuising the outdate preparation time key, otherwise create a new one
                var lastKey = _bookings.Count();
                var removeEnumerator = keysToRemove.SelectMany(keys => keys).ToList().GetEnumerator();
                bookingsTemporaryStorage.ForEach((b) =>
                {
                    if (removeEnumerator.MoveNext())
                    {
                        b.Id = removeEnumerator.Current;
                        _bookings.Remove(removeEnumerator.Current);
                    }
                    else {
                        b.Id = ++lastKey;
                    }

                    _bookings.Add(b.Id, b);
                });
            }

            // If everything was ok, now we can update the rental into the storage.
            _rentals[rentalId] = rentalCopy;

            return new ResourceIdViewModel { Id = rentalId };
        }
    }
}
