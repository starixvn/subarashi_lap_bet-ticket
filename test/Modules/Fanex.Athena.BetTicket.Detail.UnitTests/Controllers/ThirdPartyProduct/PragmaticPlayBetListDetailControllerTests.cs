using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct
{
    public class PragmaticPlayBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;

        public PragmaticPlayBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
        }

        [Fact]
        public void PragmaticPlayBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new PragmaticPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void PragmaticPlayBetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new PragmaticPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new PragmaticPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new PragmaticPlayBetListDetailController(null, null);

            // Assert
            Assert.NotNull(controller);
        }
    }
} 