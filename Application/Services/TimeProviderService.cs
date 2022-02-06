using System;
using Application.Services.Interfaces;

namespace Application.Services
{
    public class TimeProviderService : ITimeProviderService
    {
        public DateTime Now()
        {
            return new DateTime(2022, 2, 6);
        }
    }
}