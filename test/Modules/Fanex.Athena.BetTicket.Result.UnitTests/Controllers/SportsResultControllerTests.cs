using Fanex.Athena.BetTicket.Result.Controllers;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Result.MetaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Result.UnitTests.Controllers
{
    public class SportsResultControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;

        public SportsResultControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
        }

        [Fact]
        public void SportsResultController_ShouldInheritFromController()
        {
            // Arrange & Act
            var controller = new SportsResultController();

            // Assert
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Constructor_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new SportsResultController();

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Index_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new SportsResultController();
            var methodInfo = typeof(SportsResultController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(SportsResultController);

            // Act & Assert
            var authorizeAttribute = Attribute.GetCustomAttribute(controllerType, typeof(AuthorizeAttribute));
            Assert.NotNull(authorizeAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveResponseCacheAttribute()
        {
            // Arrange
            var controllerType = typeof(SportsResultController);

            // Act & Assert
            var responseCacheAttribute = Attribute.GetCustomAttribute(controllerType, typeof(ResponseCacheAttribute));
            Assert.NotNull(responseCacheAttribute);
        }
    }
} 