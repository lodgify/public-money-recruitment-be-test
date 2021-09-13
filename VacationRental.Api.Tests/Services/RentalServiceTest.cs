using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Application;
using Xunit;

namespace VacationRental.Api.Tests.Services
{
    public class RentalServiceTest
    {
        private IRentalService _iRentalService;

        public RentalServiceTest(IRentalService iRentalService)
        {
            _iRentalService = iRentalService;
        }
        
        public void AddRental()
        {
            var response = _iRentalService.AddRental(new AddRentalRequest() { PreparationTimeInDays =2, Units = 1 });

            Assert.True(response.Success);
        }
    }
}
