using Xunit;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using System.Net.Http;
using VacationRental.Api.Tests.Helpers;
using Code = VacationRental.Api.Tests.Helpers.Common;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;
using VacationRental.Api.Models.Common;
using System;

namespace VacationRental.Api.Tests
{
    public class PutRentalTest
    {
        private readonly HttpClientHelper client;

        public PutRentalTest()
        {
            client = new HttpClientHelper(new IntegrationFixture().Client);
        }


        [Fact]
        public async Task ChangeRentalUnits_ZeroBooking_AwaitSuccess() 
        {
            var rental = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var getRentalResult = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", Code.OK);
            
            Assert.Equal(getRentalResult.Units, rental.Units);
            Assert.Equal(getRentalResult.PreparationTimeInDays, rental.PreparationTimeInDays);

            var putRental = new RentalBindingModel
            {
                Units = 10, 
                PreparationTimeInDays = 2
            };
            var putRentalResult = await client.PutAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", putRental, Code.OK);
            
            Assert.Equal(putRentalResult.Units, putRental.Units);
            Assert.Equal(putRentalResult.PreparationTimeInDays, putRental.PreparationTimeInDays);
            Assert.NotEqual(putRentalResult.Units, getRentalResult.Units);
            Assert.NotEqual(putRentalResult.PreparationTimeInDays, getRentalResult.PreparationTimeInDays);
        }

        [Fact]
        public async Task ChangeRental_ZeroUnits_AwaitFail()
        {
            var rental = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var getRentalResult = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", Code.OK);

            Assert.Equal(getRentalResult.Units, rental.Units);
            Assert.Equal(getRentalResult.PreparationTimeInDays, rental.PreparationTimeInDays);
            var putRental = new RentalBindingModel
            {
                Units = 0,
                PreparationTimeInDays = 2
            };
            var putRentalResult = await client.PutAsync<ExceptionViewModel>($"/api/v1/rentals/{postRentalResult.Id}", putRental, Code.Unprocessable);
            Assert.Equal(putRentalResult.Message, Error.RentalUnitsZero);
        }

        [Fact]
        public async Task ChangeRental_LessZeroUnits_AwaitFail()
        {
            var rental = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var getRentalResult = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", Code.OK);

            Assert.Equal(getRentalResult.Units, rental.Units);
            Assert.Equal(getRentalResult.PreparationTimeInDays, rental.PreparationTimeInDays);
            var putRental = new RentalBindingModel
            {
                Units = -100,
                PreparationTimeInDays = 2
            };
            var putRentalResult = await client.PutAsync<ExceptionViewModel>($"/api/v1/rentals/{postRentalResult.Id}", putRental, Code.Unprocessable);
            Assert.Equal(putRentalResult.Message, Error.RentalUnitsZero);
        }

        [Fact]
        public async Task ChangeRental_LessZeroPreparationTime_AwaitFail()
        {
            var rental = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var getRentalResult = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", Code.OK);

            Assert.Equal(getRentalResult.Units, rental.Units);
            Assert.Equal(getRentalResult.PreparationTimeInDays, rental.PreparationTimeInDays);


            var putRental = new RentalBindingModel
            {
                Units = 100,
                PreparationTimeInDays = -100
            };
            var putRentalResult = await client.PutAsync<ExceptionViewModel>($"/api/v1/rentals/{postRentalResult.Id}", putRental, Code.Unprocessable);
            Assert.Equal(putRentalResult.Message, Error.PreparationTimeLessZero);
        }

        [Fact]
        public async Task IncrementRentalUnits_3Bookings_StartAndEndDatesNotIntersect_AwaitSuccess()
        {
            var rental = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 0
            };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var getRentalResult = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", Code.OK);
            Assert.Equal(getRentalResult.Units, rental.Units);
            Assert.Equal(getRentalResult.PreparationTimeInDays, rental.PreparationTimeInDays);

            int nights = 1;
            for (int i = 0; i < 3; i++) 
            {
                var booking = new BookingBindingModel
                {
                    RentalId = postRentalResult.Id,
                    Nights = nights - i,
                    Start = DateTime.Now.AddDays(nights + i)
                };
                var postBooking = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking, Code.OK);
                Assert.Equal(postBooking.Id, nights);
                nights++;
            }


            var putRental = new RentalBindingModel
            {
                Units = 3,
                PreparationTimeInDays = 0
            };

            var putRentalResult = await client.PutAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", putRental, Code.OK);
            Assert.Equal(putRentalResult.Units, putRental.Units);
            Assert.Equal(putRentalResult.PreparationTimeInDays, putRental.PreparationTimeInDays);
        }

        [Fact]
        public async Task DecrementRentalUnits_3Bookings_OneBookingIntersect_AwaitFail()
        {
            var rental = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 0
            };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var getRentalResult = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", Code.OK);
            Assert.Equal(getRentalResult.Units, rental.Units);
            Assert.Equal(getRentalResult.PreparationTimeInDays, rental.PreparationTimeInDays);


            var booking1 = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.Now
            };            
            var booking2 = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.Now
            };
            var booking3 = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.Now.AddDays(5)
            };

            var postBooking1 = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking1, Code.OK);
            var postBooking2 = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking2, Code.OK);
            var postBooking3 = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking3, Code.OK);

            Assert.Equal(1, postBooking1.Id);
            Assert.Equal(2, postBooking2.Id);
            Assert.Equal(3, postBooking3.Id);


            var putRental = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 0
            };

            var putRentalResult = await client.PutAsync<ExceptionViewModel>($"/api/v1/rentals/{postRentalResult.Id}", putRental, Code.Unprocessable);
            Assert.Equal(putRentalResult.Message, Error.UnallowedRentalEditing);
        }

        [Fact]
        public async Task IncrementRentalPreparation_3Bookings_OneBookingIntersect_AwaitFail()
        {
            var rental = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 0
            };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", rental, Code.OK);
            var getRentalResult = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRentalResult.Id}", Code.OK);
            Assert.Equal(getRentalResult.Units, rental.Units);
            Assert.Equal(getRentalResult.PreparationTimeInDays, rental.PreparationTimeInDays);


            var booking1 = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.Now
            };
            var booking2 = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.Now.AddDays(2)
            };


            var postBooking1 = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking1, Code.OK);
            var postBooking2 = await client.PostAsync<ResourceIdViewModel>("/api/v1/bookings", booking2, Code.OK);

            Assert.Equal(1, postBooking1.Id);
            Assert.Equal(2, postBooking2.Id);


            rental.PreparationTimeInDays = 1;

            var putRentalResult = await client.PutAsync<ExceptionViewModel>($"/api/v1/rentals/{postRentalResult.Id}", rental, Code.Unprocessable);
            Assert.Equal(putRentalResult.Message, Error.UnallowedRentalEditing);
        }
    }
}
