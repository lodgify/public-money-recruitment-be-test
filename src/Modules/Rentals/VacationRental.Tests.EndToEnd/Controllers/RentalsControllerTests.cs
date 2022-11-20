using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using VacationRental.Api;
using VacationRental.Api.Models;
using VacationRental.Application.Commands;
using VacationRental.Core.Entities;
using VacationRental.Tests.EndToEnd.Common;

namespace VacationRental.Tests.EndToEnd.Controllers
{
    public class RentalsControllerTests : WebApiTestBase
    {
        [Fact]
        public async Task given_valid_command_add_rental_should_succeed()
        {
            await _dbContext.Context.Database.EnsureCreatedAsync();

            const int units = 1;
            const int preparationTimeInDays = 2;

            var command = new AddRental(units, preparationTimeInDays);
            var response = await PostAsync("", command);

            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task given_valid_query_get_rental_should_succeed()
        {
            await _dbContext.Context.Database.EnsureCreatedAsync();

            var rental = new Rental(_fixture.Create<int>(), _fixture.Create<int>());
            await _dbContext.Context.Rentals.AddAsync(rental);
            await _dbContext.Context.SaveChangesAsync();

            var rentalResponse = await GetAsync<RentalViewModel>(rental.Id.ToString());

            rentalResponse.Id.ShouldBe(rental.Id);
            rentalResponse.Units.ShouldBe(rental.Units);
        }

        private readonly TestRentalsDbContext _dbContext;
        private readonly Fixture _fixture;

        public RentalsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            _dbContext = new TestRentalsDbContext();
            _fixture = new Fixture();
            SetPath("api/v1/rentals");
        }

        public override void Dispose()
        {
            _dbContext.Dispose();
            base.Dispose();
        }
    }
}
