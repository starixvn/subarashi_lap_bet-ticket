using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct
{
    public class SGJackpotWonBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;
        public SGJackpotWonBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();

                }

        [Fact]
        public void SGJackpotWonBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new SGJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void SGJackpotWonBetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new SGJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_Jackpot_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new SGJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new SGJackpotWonBetListDetailController(null, null, null);

            // Assert
            Assert.NotNull(controller);
        }
    }
} 