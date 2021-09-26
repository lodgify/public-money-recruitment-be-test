using System;
using Microsoft.Extensions.Logging;

namespace VacationRental.UnitTests.Application.Aspects.ErrorHandler
{

    //ILogger<T>.LogError is an extension method for ILogger so we can't create a fake to see whether the logger has been called
    public class TestLogger : ILogger<TestRequest>
    {
        public bool LogHasBeenCalled { get; private set; }
        public LogLevel LogLevelValueForLogMethod { get; private set; }
        public string ErrorMessageParameter { get; private set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogHasBeenCalled = true;
            LogLevelValueForLogMethod = logLevel;
            ErrorMessageParameter = state.ToString();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
