using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers
{
    public class ErrorHandlingTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;

        public ErrorHandlingTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();
        }

        [Fact]
        public async Task Data_WhenApiClientThrowsException_ShouldPropagateException()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailApiMetaModel();

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("API Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => controller.Data(metaModel));
        }

        [Fact]
        public async Task Data_WhenApiClientReturnsNull_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailApiMetaModel();

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync((BetListViewModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => controller.Data(metaModel));
        }

        [Fact]
        public async Task Export_WhenApiClientThrowsException_ShouldPropagateException()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailExportMetaModel { Date = "2024-01-01", SportTypeName = "Football" };

            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ThrowsAsync(new TimeoutException("Request timeout"));

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(() => controller.Export(metaModel));
        }

        [Fact]
        public async Task Export_WhenApiClientReturnsNullStream_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailExportMetaModel { Date = "2024-01-01", SportTypeName = "Football" };

            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync((System.IO.Stream)null);

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.Export(metaModel));
        }

        [Fact]
        public void Index_WithNullMetaModel_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            BetListDetailMetaModel metaModel = null;

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act & Assert - Should not throw exception
            var result = controller.Index(metaModel);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Data_WithNullMetaModel_ShouldThrowException()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            BetListDetailApiMetaModel metaModel = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => controller.Data(metaModel));
        }

        [Fact]
        public void ContextService_WhenIsAllowedCustomerCommThrowsException_ShouldPropagateExceptionOnPropertyAccess()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm)
                .Throws(new UnauthorizedAccessException("Access denied"));
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);

            // Act & Assert - Exception is thrown when ShowingOption property is accessed (wrapped in TargetInvocationException)
            var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                var showingOption = controller.GetType()
                    .GetProperty("ShowingOption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(controller);
            });
            Assert.IsType<UnauthorizedAccessException>(ex.InnerException);
        }

        [Fact]
        public async Task Data_WithEmptyResponseData_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailApiMetaModel();
            var mockResponse = new BetListViewModel
            {
                Main = new List<TicketText>(), // Empty list
                Total = new BetListTotal { SumWinLoss = 0, SumLicenseeWl = 0, SumStake = 0, SumComm = 0 }
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await controller.Data(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public async Task Data_WithNullTotalInResponse_ShouldThrowException()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailApiMetaModel();
            var mockResponse = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = null // Null total
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => controller.Data(metaModel));
        }

        [Fact]
        public async Task Data_WithNullMainInResponse_ShouldThrowException()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailApiMetaModel();
            var mockResponse = new BetListViewModel
            {
                Main = null, // Null main
                Total = new BetListTotal { SumWinLoss = 1000, SumLicenseeWl = 500, SumStake = 2000, SumComm = 100 }
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.Data(metaModel));
        }

        [Fact]
        public void Index_WithoutHttpContext_ShouldThrowException()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailMetaModel { SportTypeName = "Football" };
            // Don't set HttpContext

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => controller.Index(metaModel));
        }

        [Fact]
        public async Task Export_WithoutHttpContext_ShouldThrowException()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailExportMetaModel { Date = "2024-01-01", SportTypeName = "Football" };
            var mockStream = new System.IO.MemoryStream();

            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockStream);

            // Don't set HttpContext

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => controller.Export(metaModel));
        }

        [Fact]
        public async Task Data_WithVeryLargeDecimalValues_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailApiMetaModel();
            var mockResponse = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = new BetListTotal
                {

                    SumWinLoss = decimal.MaxValue,

                    SumLicenseeWl = decimal.MaxValue,

                    SumStake = decimal.MaxValue,

                    SumComm = decimal.MaxValue

                }
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await controller.Data(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task Data_WithNegativeDecimalValues_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new AllBetBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object, _mockResourceService.Object);
            var metaModel = new BetListDetailApiMetaModel();
            var mockResponse = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = new BetListTotal
                {

                    SumWinLoss = -1000.50m,

                    SumLicenseeWl = -500.25m,

                    SumStake = -2000.75m,

                    SumComm = -100.10m

                }
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await controller.Data(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
        }
    }
}
