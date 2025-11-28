using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers
{
    public class SystemParlayBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;

        public SystemParlayBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();
        }

        [Fact]
        public void SystemParlayBetListDetailController_ShouldInheritFromController()
        {
            // Arrange & Act
            var controller = new SystemParlayBetListDetailController();

            // Assert
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Constructor_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new SystemParlayBetListDetailController();

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Index_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new SystemParlayBetListDetailController();
            var methodInfo = typeof(SystemParlayBetListDetailController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void Index_ShouldHaveAutoBindingMetaModelAttribute()
        {
            // Arrange
            var controller = new SystemParlayBetListDetailController();
            var methodInfo = typeof(SystemParlayBetListDetailController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var autoBindingAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(AutoBindingMetaModelAttribute));
            Assert.NotNull(autoBindingAttribute);
        }

        [Fact]
        public void Index_ShouldHaveResponseCacheAttribute()
        {
            // Arrange
            var controller = new SystemParlayBetListDetailController();
            var methodInfo = typeof(SystemParlayBetListDetailController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var responseCacheAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(ResponseCacheAttribute));
            Assert.NotNull(responseCacheAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(SystemParlayBetListDetailController);

            // Act & Assert
            var authorizeAttribute = Attribute.GetCustomAttribute(controllerType, typeof(AuthorizeAttribute));
            Assert.NotNull(authorizeAttribute);
        }
    }
}