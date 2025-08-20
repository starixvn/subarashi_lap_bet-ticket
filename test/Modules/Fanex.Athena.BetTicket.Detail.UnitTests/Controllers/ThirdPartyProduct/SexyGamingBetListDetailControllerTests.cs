using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct
{
    public class SexyGamingBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;

        public SexyGamingBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
        }

        [Fact]
        public void SexyGamingBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void SexyGamingBetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_LC_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new SexyGamingBetListDetailController(null, null);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void HiddenColumns_ShouldReturnLicenseeWLColumn()
        {
            // Arrange
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var hiddenColumns = controller.GetType().GetProperty("HiddenColumns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as int[];

            // Assert
            Assert.NotNull(hiddenColumns);
            Assert.Single(hiddenColumns);
            Assert.Contains(BetListColumn.LicenseeWLColumn, hiddenColumns);
        }

        [Fact]
        public void ProductName_ShouldReturnSexyGaming()
        {
            // Arrange
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var productName = controller.GetType().GetProperty("ProductName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as string;

            // Assert
            Assert.Equal(ViewResources.ProductSXG, productName);
        }

        [Fact]
        public void ExcelFileName_ShouldReturnAESexyDetail()
        {
            // Arrange
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var excelFileName = controller.GetType().GetProperty("ExcelFileName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as string;

            // Assert
            Assert.Equal("AESexyDetail", excelFileName);
        }

        [Fact]
        public void ShowingOption_WhenCustomerCommAllowed_ShouldHaveCorrectSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var showingOption = controller.GetType().GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.True(showingOption.GrandTotalVisible); // Default value from ShowingOption constructor
            Assert.True(showingOption.CustomerCommVisible);
        }

        [Fact]
        public void ShowingOption_WhenCustomerCommNotAllowed_ShouldHaveCorrectSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            var controller = new SexyGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var showingOption = controller.GetType().GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.True(showingOption.GrandTotalVisible); // Default value from ShowingOption constructor
            Assert.False(showingOption.CustomerCommVisible);
        }
    }
}