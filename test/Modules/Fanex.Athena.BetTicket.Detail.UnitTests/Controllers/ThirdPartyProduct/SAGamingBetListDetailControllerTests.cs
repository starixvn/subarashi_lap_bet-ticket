using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct
{
    public class SAGamingBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;
        public SAGamingBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();

                }

        [Fact]
        public void SAGamingBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void SAGamingBetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_LC_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new SAGamingBetListDetailController(null, null, null);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void HiddenColumns_ShouldReturnLicenseeWLColumn()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var hiddenColumns = controller.GetType().GetProperty("HiddenColumns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as int[];

            // Assert
            Assert.NotNull(hiddenColumns);
            Assert.Single(hiddenColumns);
            Assert.Contains(BetListColumn.LicenseeWLColumn, hiddenColumns);
        }

        [Fact]
        public void ProductName_ShouldReturnSAGaming()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var productName = controller.GetType().GetProperty("ProductName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as string;

            // Assert
            Assert.Equal("SA Gaming", productName);
        }

        [Fact]
        public void ExcelFileName_ShouldReturnSAGamingDetail()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var excelFileName = controller.GetType().GetProperty("ExcelFileName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as string;

            // Assert
            Assert.Equal("SAGamingDetail", excelFileName);
        }

        [Fact]
        public void ShowingOption_WhenCustomerCommAllowed_ShouldHaveCorrectSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var showingOption = controller.GetType().GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.True(showingOption.CustomerCommVisible);
        }

        [Fact]
        public void ShowingOption_WhenCustomerCommNotAllowed_ShouldHaveCorrectSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var showingOption = controller.GetType().GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.False(showingOption.CustomerCommVisible);
        }

        [Fact]
        public void Index_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailMetaModel();

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = controller.Index(metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ShouldSetViewBagProperties()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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
            Assert.Equal("Lrf_BetList_LC_Detail", metaModel.UspKey);
        }

        [Fact]
        public async Task Export_ShouldReturnFileResult()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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

        [Fact]
        public async Task Export_ShouldSetMetaModelProperties()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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
            await controller.Export(metaModel);

            // Assert
            Assert.Equal("Lrf_BetList_LC_Detail", metaModel.UspKey);
            Assert.NotNull(metaModel.ShowingOption);
            Assert.NotNull(metaModel.HiddenColumns);
            Assert.Equal("Football - Detailed Bet list - 2024-01-01", metaModel.Title);
        }

        [Fact]
        public void BuildTitle_WithSportTypeName_ShouldReturnFormattedTitle()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var betDate = "2024-01-01";
            var sportTypeName = "Football";

            // Act
            var result = controller.GetType().GetMethod("BuildTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(controller, new object[] { betDate, sportTypeName }) as string;

            // Assert
            Assert.Equal("Football - Detailed Bet list - 2024-01-01", result);
        }

        [Fact]
        public void BuildTitle_WithoutSportTypeName_ShouldReturnFormattedTitle()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var betDate = "2024-01-01";

            // Act
            var result = controller.GetType().GetMethod("BuildTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(controller, new object[] { betDate, null }) as string;

            // Assert
            Assert.Equal("SA Gaming - Detailed Bet list - 2024-01-01", result);
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(SAGamingBetListDetailController);

            // Act & Assert
            var authorizeAttribute = System.Attribute.GetCustomAttribute(controllerType, typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute));
            Assert.NotNull(authorizeAttribute);
        }

        [Fact]
        public void Index_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var methodInfo = typeof(BetListDetailControllerBase).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = System.Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void Data_ShouldHaveHttpPostAttribute()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var methodInfo = typeof(BetListDetailControllerBase).GetMethod("Data");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpPostAttribute = System.Attribute.GetCustomAttribute(methodInfo, typeof(HttpPostAttribute));
            Assert.NotNull(httpPostAttribute);
        }

        [Fact]
        public void Export_ShouldHaveHttpPostAttribute()
        {
            // Arrange
            var controller = new SAGamingBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var methodInfo = typeof(BetListDetailControllerBase).GetMethod("Export");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpPostAttribute = System.Attribute.GetCustomAttribute(methodInfo, typeof(HttpPostAttribute));
            Assert.NotNull(httpPostAttribute);
        }
    }
} 