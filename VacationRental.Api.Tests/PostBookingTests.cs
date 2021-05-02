using Xunit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Models.Common;
using VacationRental.Api.Tests.Helpers;
using Code = VacationRental.Api.Tests.Helpers.Common;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task RegisterBooking_NoExistingRental_AwaitFail()
        {
            var booking = new BookingBindingModel
            {
                RentalId = 999,
                Nights = 3,
                Start = DateTime.Now.AddYears(10)
            };
            var postBooking = await HttpClientHelper.Post<ExceptionViewModel>("/api/v1/bookings", booking, _client, Code.Unprocessable);
            Assert.Equal(postBooking.Message, Error.RentalNotFound);
        }

        [Fact]
        public async Task RegisterBooking_ZeroRentalId_AwaitFail()
        {
            var booking = new BookingBindingModel
            {
                RentalId = 0,
                Nights = 3,
                Start = DateTime.Now.AddYears(10)
            };
            var postBooking = await HttpClientHelper.Post<ExceptionViewModel>("/api/v1/bookings", booking, _client, Code.Unprocessable);
            Assert.Equal(postBooking.Message, Error.RentalIdLessOrZero);
        }
        
        [Fact]
        public async Task RegisterBooking_LessZeroRentalId_AwaitFail()
        {
            var booking = new BookingBindingModel
            {
                RentalId = -999,
                Nights = 3,
                Start = DateTime.Now.AddYears(10)
            };
            var postBooking = await HttpClientHelper.Post<ExceptionViewModel>("/api/v1/bookings", booking, _client, Code.Unprocessable);
            Assert.Equal(postBooking.Message, Error.RentalIdLessOrZero);
        }

        [Fact]
        public async Task RegisterOneUnitRental_RegisterOneBooking_AwaitSuccess()
        {
            var rental = new RentalBindingModel { Units = 4 };
            var postRental = await HttpClientHelper.Post<ResourceIdViewModel>("/api/v1/rentals", rental, _client, Code.OK);
            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 3,
                Start = DateTime.Now.AddYears(10)
            };

            var postBooking = await HttpClientHelper.Post<ResourceIdViewModel>("/api/v1/bookings", booking, _client, Code.OK);
            var getBookingResult = await HttpClientHelper.Get<BookingViewModel>($"/api/v1/bookings/{postBooking.Id}", _client, Code.OK);

            Assert.Equal(booking.RentalId, getBookingResult.RentalId);
            Assert.Equal(booking.Nights, getBookingResult.Nights);
            Assert.Equal(booking.Start, getBookingResult.Start);
        }

        [Fact]
        public async Task RegisterTwoBookingInSameTim_OneUnit_AwaitFail()
        {
            var rental = new RentalBindingModel { Units = 1 };
            var postRental = await HttpClientHelper.Post<ResourceIdViewModel>("/api/v1/rentals", rental, _client);

            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 15,
                Start = DateTime.Now.AddYears(10)
            };

            var postBooking1 = await HttpClientHelper.Post<ResourceIdViewModel>("/api/v1/bookings", booking, _client, Code.OK);
            var postBooking2 = await HttpClientHelper.Post<ExceptionViewModel>("/api/v1/bookings", booking, _client, Code.Unprocessable);

            Assert.Equal(postBooking2.Message, Error.NoVacancy);
        }

        [Fact]
        public async Task RegisterTwoBooking_OneUnit_FirstBookingEndDate_Same_SecondStartDate_AwaitFail() 
        {
        }
    }
}
