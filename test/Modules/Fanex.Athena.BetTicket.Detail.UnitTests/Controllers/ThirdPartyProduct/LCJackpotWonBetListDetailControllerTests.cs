using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
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
    public class LCJackpotWonBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;
        public LCJackpotWonBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();

                }

        [Fact]
        public void LCJackpotWonBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void LCJackpotWonBetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_Jackpot_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new LCJackpotWonBetListDetailController(null, null, null);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void HiddenColumns_ShouldReturnLicenseeWLColumn()
        {
            // Arrange
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var hiddenColumns = controller.GetType().GetProperty("HiddenColumns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as int[];

            // Assert
            Assert.NotNull(hiddenColumns);
            Assert.Single(hiddenColumns);
            Assert.Contains(BetListColumn.LicenseeWLColumn, hiddenColumns);
        }

        [Fact]
        public void ShowingOption_WhenCustomerCommAllowed_ShouldHaveCorrectSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

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
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act
            var showingOption = controller.GetType().GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(controller) as ShowingOption;

            // Assert
            Assert.NotNull(showingOption);
            Assert.True(showingOption.GrandTotalVisible); // Default value from ShowingOption constructor
            Assert.False(showingOption.CustomerCommVisible);
        }

        [Fact]
        public void Index_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(LCJackpotWonBetListDetailController);

            // Act & Assert
            var authorizeAttribute = System.Attribute.GetCustomAttribute(controllerType, typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute));
            Assert.NotNull(authorizeAttribute);
        }

        [Fact]
        public void Index_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
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
            var controller = new LCJackpotWonBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var methodInfo = typeof(BetListDetailControllerBase).GetMethod("Export");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpPostAttribute = System.Attribute.GetCustomAttribute(methodInfo, typeof(HttpPostAttribute));
            Assert.NotNull(httpPostAttribute);
        }
    }
}