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
    public class AdvantPlayBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;
        public AdvantPlayBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();

                }

        [Fact]
        public void AdvantPlayBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void AdvantPlayBetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new AdvantPlayBetListDetailController(null, null, null);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void HiddenColumns_ShouldReturnLicenseeWLColumn()
        {
            // Arrange
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var hiddenColumns = controller.GetType().GetProperty("HiddenColumns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as int[];

            // Assert
            Assert.NotNull(hiddenColumns);
            Assert.Single(hiddenColumns);
            Assert.Contains(BetListColumn.LicenseeWLColumn, hiddenColumns);
        }

        [Fact]
        public void ProductName_ShouldReturnAdvantPlay()
        {
            // Arrange
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var productName = controller.GetType().GetProperty("ProductName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as string;

            // Assert
            Assert.Equal(ViewResources.ProductAP, productName);
        }

        [Fact]
        public void ExcelFileName_ShouldReturnAdvantPlayDetail()
        {
            // Arrange
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var excelFileName = controller.GetType().GetProperty("ExcelFileName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as string;

            // Assert
            Assert.Equal("AdvantPlayDetail", excelFileName);
        }

        [Fact]
        public void ShowingOption_WhenCustomerCommAllowed_ShouldHaveCorrectSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

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
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var showingOption = controller.GetType().GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.True(showingOption.GrandTotalVisible); // Default value from ShowingOption constructor
            Assert.False(showingOption.CustomerCommVisible);
        }
    }
}