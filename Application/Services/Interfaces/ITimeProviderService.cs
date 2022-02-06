using System;

namespace Application.Services.Interfaces
{
    //Added to support tests
    public interface ITimeProviderService
    {
        DateTime Now();
    }
}