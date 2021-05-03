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
    public class PostBookingTests
    {
        private readonly HttpClientHelper client;

        public PostBookingTests()
        {
            client = new HttpClientHelper(new IntegrationFixture().Client);
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
            var postBooking = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking, Code.Unprocessable);
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
            var postBooking = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking, Code.Unprocessable);
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
            var postBooking = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking, Code.Unprocessable);
            Assert.Equal(postBooking.Message, Error.RentalIdLessOrZero);
        }

        [Fact]
        public async Task RegisterOneUnitRental_RegisterOneBooking_AwaitSuccess()
        {
            var rental = new RentalBindingModel { Units = 4 };
            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 3,
                Start = DateTime.Now.AddYears(10)
            };

            var postBooking = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking,  Code.OK);
            var getBookingResult = await client.GetAsync<BookingViewModel>($"/api/v1/bookings/{postBooking.Id}", Code.OK);

            Assert.Equal(booking.RentalId, getBookingResult.RentalId);
            Assert.Equal(booking.Nights, getBookingResult.Nights);
            Assert.Equal(booking.Start, getBookingResult.Start);
        }

        [Fact]
        public async Task RegisterBooking_ZeroNights_AwaitFail()
        {
            var rental = new RentalBindingModel { Units = 4 };
            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);

            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 0,
                Start = DateTime.Now.AddYears(10)
            };
            var postBooking = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking, Code.Unprocessable);
            Assert.Equal(postBooking.Message, Error.BookingNightsZero);
        }

        [Fact]
        public async Task RegisterBooking_PassedDate_AwaitFail()
        {
            var rental = new RentalBindingModel { Units = 4 };
            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 10,
                Start = DateTime.Now.AddDays(-10)
            };
            var postBooking = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking, Code.Unprocessable);
            Assert.Equal(postBooking.Message, Error.DateAlreadyPassed);
        }

        [Fact]
        public async Task RegisterBooking_Yesterday_AwaitFail()
        {
            var rental = new RentalBindingModel { Units = 4 };
            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 10,
                Start = DateTime.Now.AddDays(-1)
            };
            var postBooking = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking, Code.Unprocessable);
            Assert.Equal(postBooking.Message, Error.DateAlreadyPassed);
        }

        [Fact]
        public async Task RegisterBooking_Today_AwaitSuccess()
        {
            var rental = new RentalBindingModel { Units = 4 };
            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 10,
                Start = DateTime.Now
            };
            var postBooking = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking, Code.OK);
            var getBookingResult = await client.GetAsync<BookingViewModel>($"/api/v1/bookings/{postBooking.Id}", Code.OK);

            Assert.Equal(getBookingResult.Nights, booking.Nights);
            Assert.Equal(getBookingResult.RentalId, booking.RentalId);
            Assert.Equal(getBookingResult.Start, booking.Start);
        }

        [Fact]
        public async Task RegisterTwoBookingInSameTim_OneUnit_AwaitFail()
        {
            var rental = new RentalBindingModel { Units = 1 };
            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental);

            var booking = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = 15,
                Start = DateTime.Now.AddYears(10)
            };

            var postBooking1 = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking, Code.OK);
            var postBooking2 = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking, Code.Unprocessable);

            Assert.Equal(postBooking2.Message, Error.NoVacancy);
        }

        [Fact]
        public async Task RegisterTwoBooking_OneUnit_FirstBookingEndDate_Same_SecondStartDate_AwaitFail()
        {
            var rental = new RentalBindingModel { Units = 1 };
            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental);

            int nights = 10;
            var booking1 = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = nights,
                Start = DateTime.Now.AddDays(1)
            };
            var booking2 = new BookingBindingModel
            {
                RentalId = postRental.Id,
                Nights = nights,
                Start = DateTime.Now.AddDays(1 + nights)
            };
            var postBooking1 = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking1, Code.OK);
            var postBooking2 = await client.PostAsync<ExceptionViewModel>("/api/v1/bookings", booking2, Code.Unprocessable);
            Assert.Equal(postBooking2.Message, Error.NoVacancy);
        }


    }
}
