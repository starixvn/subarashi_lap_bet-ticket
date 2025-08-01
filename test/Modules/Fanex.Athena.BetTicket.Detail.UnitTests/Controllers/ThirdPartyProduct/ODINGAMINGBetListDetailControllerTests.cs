using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct
{
    public class ODINGAMINGBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;

        public ODINGAMINGBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
        }

        [Fact]
        public void ODINGAMINGBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullBetListApiClient_ShouldNotThrow()
        {
            // Arrange, Act & Assert - Controllers don't validate null dependencies
            var controller = new ODINGAMINGBetListDetailController(null, _mockContextService.Object);
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullContextService_ShouldNotThrow()
        {
            // Arrange, Act & Assert - Controllers don't validate null dependencies
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, null);
            Assert.NotNull(controller);
        }

        [Fact]
        public void StoreProcedure_ShouldReturnCorrectValue()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var result = controller.StoreProcedure;

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", result);
        }

        [Fact]
        public void HiddenColumns_ShouldContainLicenseeWLColumn()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var hiddenColumns = controller.GetType()
                .GetProperty("HiddenColumns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(controller) as int[];

            // Assert
            Assert.NotNull(hiddenColumns);
            Assert.Contains(BetListColumn.LicenseeWLColumn, hiddenColumns);
        }

        [Fact]
        public void ProductName_ShouldReturnCorrectValue()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var productName = controller.GetType()
                .GetProperty("ProductName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(controller) as string;

            // Assert
            Assert.Equal(ViewResources.ProductODG, productName);
        }

        [Fact]
        public void ExcelFileName_ShouldReturnCorrectValue()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var excelFileName = controller.GetType()
                .GetProperty("ExcelFileName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(controller) as string;

            // Assert
            Assert.Equal("ODINGAMINGDetail", excelFileName);
        }

        [Fact]
        public void ShowingOption_CustomerCommVisible_ShouldDependOnContextService()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var showingOption = controller.GetType()
                .GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.True(showingOption.CustomerCommVisible);
        }

        [Fact]
        public void ShowingOption_CustomerCommVisible_ShouldBeFalseWhenNotAllowed()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var showingOption = controller.GetType()
                .GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.False(showingOption.CustomerCommVisible);
        }

        [Fact]
        public void Index_ShouldSetViewBagProperties()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);
            var metaModel = new BetListDetailMetaModel { SportTypeName = "Football" };

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            controller.Index(metaModel);

            // Assert
            Assert.NotNull(controller.ViewBag.ApiUrl);
            Assert.NotNull(controller.ViewBag.ExportUrl);
            Assert.NotNull(controller.ViewBag.HiddenColumns);
            Assert.NotNull(controller.ViewBag.IsShowGrandTotal);
            Assert.NotNull(controller.ViewBag.ExportFileName);
        }

        [Fact]
        public async Task Data_ShouldReturnJsonResult()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);
            var metaModel = new BetListDetailApiMetaModel();
            var mockResponse = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = new BetListTotal { SumWinLoss = 1000, SumLicenseeWl = 500, SumStake = 2000, SumComm = 100 }
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await controller.Data(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task Data_ShouldSetMetaModelUspKey()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);
            var metaModel = new BetListDetailApiMetaModel();
            var mockResponse = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = new BetListTotal { SumWinLoss = 1000, SumLicenseeWl = 500, SumStake = 2000, SumComm = 100 }
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.Data(metaModel);

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", metaModel.UspKey);
        }

        [Fact]
        public async Task Export_ShouldReturnFileResult()
        {
            // Arrange
            var controller = new ODINGAMINGBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);
            var metaModel = new BetListDetailExportMetaModel { Date = "2024-01-01", SportTypeName = "Football" };
            var mockStream = new System.IO.MemoryStream();

            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockStream);

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await controller.Export(metaModel);

            // Assert
            Assert.IsType<FileStreamResult>(result);
        }
    }
}
