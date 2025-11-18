using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct
{
    public class Live22BetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;
        public Live22BetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();

                }

        [Fact]
        public void Live22BetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new Live22BetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void Live22BetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new Live22BetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new Live22BetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new Live22BetListDetailController(null, null, null);

            // Assert
            Assert.NotNull(controller);
        }
    }
} 