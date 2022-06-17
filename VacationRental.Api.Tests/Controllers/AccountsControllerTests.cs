using Moq;
using VacationRental.Api.Host.Controllers;
using VacationRental.Services.Interfaces;
using Xunit;

namespace VacationRental.Api.Host.UnitTests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;

        private readonly AccountsController _controller;

        public AccountsControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();

            _controller = new AccountsController(_accountServiceMock.Object);
        }
        
        [Fact]
        public async Task Account_ShouldSignInGuest_Success()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.SignInGuestAsync()).ReturnsAsync(new Models.Dtos.AccessTokenDto { 
                AccessToken = "AccessToken",
                ExpiresIn = 3600,
                RefreshToken = "RefreshToken",
                TokenType = "Bearer"
            });

            // Action
            var result = await _controller.SignInGuestAsync();

            // Assert
            Assert.NotNull(result);

            _accountServiceMock.Verify(x => x.SignInGuestAsync(), Times.Once());
        }
    }
}
