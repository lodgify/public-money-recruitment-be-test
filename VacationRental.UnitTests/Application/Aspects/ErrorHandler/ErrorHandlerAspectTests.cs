using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Aspects;
using Xunit;

namespace VacationRental.UnitTests.Application.Aspects.ErrorHandler
{
    public class ErrorHandlerAspectTests
    {
        [Fact]
        public async Task Handle_NextDelegateThrowsDomainException_ApplicationExceptionShouldBeThrownInstead()
        {
            var fakeLogger = new TestLogger();
            var handler = new ErrorHandlerAspect<TestRequest, TestResponse>(fakeLogger);
            
            var exception = await Assert.ThrowsAsync<ApplicationException>(async ()=> await handler.Handle(new TestRequest(), 
                CancellationToken.None, 
                () => throw new TestDomainException()));
            
            Assert.Equal(TestDomainException.ErrorMessage, exception.Message);
        }

        [Fact]
        public async Task Handle_NextDelegateThrowsExceptionThrown_ExceptionRethrown()
        {
            var fakeLogger = new TestLogger();
            var exceptionToBeThrown = new InvalidOperationException("UnknownError");
            var handler = new ErrorHandlerAspect<TestRequest, TestResponse>(fakeLogger);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(new TestRequest(), 
                CancellationToken.None, 
                () => throw exceptionToBeThrown));

            Assert.Equal(exceptionToBeThrown.Message, exception.Message);
        }

        [Fact]
        public async Task Handle_NextDelegateThrowsDomainException_LogErrorIsCalled()
        {
            var fakeLogger = new TestLogger();
            var handler = new ErrorHandlerAspect<TestRequest, TestResponse>(fakeLogger);

            await Assert.ThrowsAsync<ApplicationException>(async () => await handler.Handle(new TestRequest(), 
                CancellationToken.None, 
                () => throw new TestDomainException()));

            Assert.True(fakeLogger.LogHasBeenCalled);
            Assert.Equal(LogLevel.Error, fakeLogger.LogLevelValueForLogMethod);
            Assert.Equal(TestDomainException.ErrorMessage, fakeLogger.ErrorMessageParameter);
        }


        [Fact]
        public async Task Handle_NextDelegateThrowsAnyException_LogErrorIsCalled()
        {
            var fakeLogger = new TestLogger();
            var handler = new ErrorHandlerAspect<TestRequest, TestResponse>(fakeLogger);
            var exceptionToBeThrown = new InvalidOperationException("UnknownError");

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(new TestRequest(),
                CancellationToken.None,
                () => throw exceptionToBeThrown));

            Assert.True(fakeLogger.LogHasBeenCalled);
            Assert.Equal(LogLevel.Error, fakeLogger.LogLevelValueForLogMethod);
            Assert.Equal(exceptionToBeThrown.Message, fakeLogger.ErrorMessageParameter);
        }
    }
}
