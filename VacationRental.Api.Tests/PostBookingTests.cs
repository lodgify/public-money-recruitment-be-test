using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

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
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBookingWhenThereIsASpareRental()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2021, 08, 14)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2021, 08, 15)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBookingResult = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBooking2Request.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBooking2Request.Nights, getBookingResult.Nights);
                Assert.Equal(postBooking2Request.Start, getBookingResult.Start);
            }
        }
        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenNoSideEffects()
        {
            //Create rentals
            var postRentalRequest1 = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult1;
            using (var postRentalResponse1 = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest1))
            {
                Assert.True(postRentalResponse1.IsSuccessStatusCode);
                postRentalResult1 = await postRentalResponse1.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postRentalRequest2 = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult2;
            using (var postRentalResponse2 = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest2))
            {
                Assert.True(postRentalResponse2.IsSuccessStatusCode);
                postRentalResult2 = await postRentalResponse2.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            //Create bookings
            var postBookingRequest1 = new BookingBindingModel
            {
                RentalId = postRentalResult1.Id,
                Nights = 5,
                Start = new DateTime(2021, 08, 15)
            };

            ResourceIdViewModel postBookingResult1;
            using (var postBookingResponse1 = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest1))
            {
                Assert.True(postBookingResponse1.IsSuccessStatusCode);
                postBookingResult1 = await postBookingResponse1.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest2 = new BookingBindingModel
            {
                RentalId = postRentalResult2.Id,
                Nights = 1,
                Start = new DateTime(2021, 08, 16)
            };

            ResourceIdViewModel postBookingResult2;
            using (var postBookingResponse2 = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest2))
            {
                Assert.True(postBookingResponse2.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse2.Content.ReadAsAsync<ResourceIdViewModel>();
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBookingWhenIsBeforeExistingBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 14),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 10),
                Nights = 4
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            ResourceIdViewModel postNewBookingResult;
            using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
            {
                Assert.True(postNewBookingResponse.IsSuccessStatusCode);
                postNewBookingResult = await postNewBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postNewBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(newBooking.RentalId, getBookingResult.RentalId);
                Assert.Equal(newBooking.Nights, getBookingResult.Nights);
                Assert.Equal(newBooking.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenBookingIsOverlapingStartOfExistingBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 14),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 10),
                Nights = 5
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenBookingIsCoveringExistingBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 14),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 10),
                Nights = 10
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenBookingDataIsInvalid()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 14),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 15),
                Nights = -7
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenBookingIsWithinExistingBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 14),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 15),
                Nights = 1
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenBookingIsOverlapingEndOfExistingBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 14),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 16),
                Nights = 5
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBookingWhenIsAfterExistingBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 14),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 17),
                Nights = 5
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            ResourceIdViewModel postNewBookingResult;
            using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
            {
                Assert.True(postNewBookingResponse.IsSuccessStatusCode);
                postNewBookingResult = await postNewBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postNewBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(newBooking.RentalId, getBookingResult.RentalId);
                Assert.Equal(newBooking.Nights, getBookingResult.Nights);
                Assert.Equal(newBooking.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsBookingsInCorrectUnit()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 3
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 17),
                Nights = 5
            };

            ResourceIdViewModel postBookingResult1;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult1 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            ResourceIdViewModel postBookingResult2;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            ResourceIdViewModel postBookingResult3;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult3 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingViewModel getBookingResult1;
            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult1.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);
                getBookingResult1 = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
            }

            BookingViewModel getBookingResult2;
            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult2.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);
                getBookingResult2 = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
            }

            BookingViewModel getBookingResult3;
            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult3.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);
                getBookingResult3 = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
            }

            Assert.True(getBookingResult1.Unit != getBookingResult2.Unit 
                && getBookingResult1.Unit != getBookingResult3.Unit
                && getBookingResult2.Unit != getBookingResult3.Unit);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenIsInPreparation()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2021, 08, 15)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2021, 08, 19)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenPreparationDoesNotAffectSpareUnit()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2021, 08, 15)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2021, 08, 19)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenBookingIsOverlapingPreparationTime()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            BookingBindingModel existingBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 16),
                Nights = 3
            };

            BookingBindingModel newBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Start = new DateTime(2021, 08, 13),
                Nights = 3
            };

            using (var postExistingBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", existingBooking))
            {
                Assert.True(postExistingBookingResponse.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postNewBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", newBooking))
                {
                }
            });
        }
    }
}
