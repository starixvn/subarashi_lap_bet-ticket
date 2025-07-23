using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.Core.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct
{
    public class AdvantPlayBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly AdvantPlayBetListDetailController _controller;

        public AdvantPlayBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockHttpContext = new Mock<HttpContext>();
            
            _controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);
            
            // Setup HttpContext for the controller
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };
        }

        #region Constructor Tests

        [Fact]
        public void AdvantPlayBetListDetailController_ShouldInheritFromBase()
        {
            // Arrange & Act
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.IsAssignableFrom<BetListDetailControllerBase>(controller);
        }

        [Fact]
        public void AdvantPlayBetListDetailController_ShouldHaveCorrectStoreProcedure()
        {
            // Arrange & Act
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", controller.StoreProcedure);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new AdvantPlayBetListDetailController(null, null);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullBetListApiClient_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new AdvantPlayBetListDetailController(null, _mockContextService.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullContextService_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, null);

            // Assert
            Assert.NotNull(controller);
        }

        #endregion

        #region Property Tests

        [Fact]
        public void StoreProcedure_ShouldReturnCorrectValue()
        {
            // Arrange
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Act
            var storeProcedure = controller.StoreProcedure;

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", storeProcedure);
        }



        #endregion

        #region Index Method Tests

        [Fact]
        public void Index_WithValidMetaModel_ShouldReturnViewResult()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                SportTypeName = "Test Sport"
            };

            // Act
            var result = _controller.Index(metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Equal("~/Views/ThirdPartyProductBetListDetail/Index.cshtml", viewResult.ViewName);
            Assert.Equal(metaModel, viewResult.Model);
        }

        [Fact]
        public void Index_WithNullMetaModel_ShouldReturnViewResult()
        {
            // Arrange
            BetListDetailMetaModel metaModel = null;

            // Act
            var result = _controller.Index(metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Equal("~/Views/ThirdPartyProductBetListDetail/Index.cshtml", viewResult.ViewName);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public void Index_ShouldSetViewBagProperties()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                SportTypeName = "Test Sport"
            };

            // Setup HttpContext for route values
            var routeValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary
            {
                { "controller", "AdvantPlayBetListDetail" }
            };
            _mockHttpContext.Setup(x => x.Request.RouteValues).Returns(routeValues);

            // Act
            var result = _controller.Index(metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Equal("AdvantPlayBetListDetail/Data", viewResult.ViewData["ApiUrl"]);
            Assert.Equal("AdvantPlayBetListDetail/Export", viewResult.ViewData["ExportUrl"]);
            Assert.Equal("AdvantPlayDetail", viewResult.ViewData["ExportFileName"]);
        }

        #endregion

        #region Data Method Tests

        [Fact]
        public async Task Data_WithValidMetaModel_ShouldReturnJsonResult()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                CustId = 12345
            };

            var responseData = new BetListViewModel
            {
                Main = new List<TicketText>
                {
                    new TicketText { TransTime = "2024-01-01", Stake = "100.00" }
                },
                Total = new BetListTotal
                {
                    SumWinLoss = 50.0m,
                    SumLicenseeWl = 25.0m,
                    SumStake = 100.0m,
                    SumComm = 5.0m
                }
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // Act
            var result = await _controller.Data(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public async Task Data_ShouldSetUspKeyToStoreProcedure()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                CustId = 12345
            };

            var responseData = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = new BetListTotal()
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // Act
            await _controller.Data(metaModel);

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", metaModel.UspKey);
        }

        [Fact]
        public async Task Data_ShouldCallApiWithCorrectParameters()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                CustId = 12345
            };

            var responseData = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = new BetListTotal()
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // Act
            await _controller.Data(metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<BetListViewModel>("/BetList/GetBetList", metaModel, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Data_WithEmptyResponse_ShouldReturnJsonResult()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                CustId = 12345
            };

            var responseData = new BetListViewModel
            {
                Main = new List<TicketText>(),
                Total = new BetListTotal()
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // Act
            var result = await _controller.Data(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public async Task Data_WithNullResponse_ShouldHandleGracefully()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                CustId = 12345
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync((BetListViewModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _controller.Data(metaModel));
        }

        #endregion

        #region Export Method Tests

        [Fact]
        public async Task Export_WithValidMetaModel_ShouldReturnFileResult()
        {
            // Arrange
            var metaModel = new BetListDetailExportMetaModel
            {
                Date = "2024-01-01"
            };

            var mockStream = new MemoryStream();
            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockStream);

            // Act
            var result = await _controller.Export(metaModel);

            // Assert
            Assert.IsType<FileContentResult>(result);
            var fileResult = result as FileContentResult;
            Assert.Equal("application/octet-stream", fileResult.ContentType);
        }

        [Fact]
        public async Task Export_ShouldSetMetaModelProperties()
        {
            // Arrange
            var metaModel = new BetListDetailExportMetaModel
            {
                Date = "2024-01-01"
            };

            var mockStream = new MemoryStream();
            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockStream);

            // Act
            await _controller.Export(metaModel);

            // Assert
            Assert.Equal("Lrf_BetList_RNG_Detail", metaModel.UspKey);
            Assert.NotNull(metaModel.ShowingOption);
            Assert.NotNull(metaModel.HiddenColumns);
            Assert.Contains(BetListColumn.LicenseeWLColumn, metaModel.HiddenColumns);
        }

        [Fact]
        public async Task Export_ShouldCallApiWithCorrectParameters()
        {
            // Arrange
            var metaModel = new BetListDetailExportMetaModel
            {
                Date = "2024-01-01"
            };

            var mockStream = new MemoryStream();
            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(mockStream);

            // Act
            await _controller.Export(metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostStream("/BetList/ExportExcel", metaModel, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Export_WithNullMetaModel_ShouldThrowException()
        {
            // Arrange
            BetListDetailExportMetaModel metaModel = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _controller.Export(metaModel));
        }

        #endregion



        #region Edge Cases and Error Handling

        [Fact]
        public async Task Data_WhenApiThrowsException_ShouldPropagateException()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel
            {
                Date = DateTime.Parse("2024-01-01"),
                CustId = 12345
            };

            _mockBetListApiClient.Setup(x => x.Post<BetListViewModel>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("API Error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Data(metaModel));
        }

        [Fact]
        public async Task Export_WhenApiThrowsException_ShouldPropagateException()
        {
            // Arrange
            var metaModel = new BetListDetailExportMetaModel
            {
                Date = "2024-01-01"
            };

            _mockBetListApiClient.Setup(x => x.PostStream(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("API Error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Export(metaModel));
        }

        [Fact]
        public void Controller_ShouldHaveCorrectType()
        {
            // Arrange & Act
            var controller = new AdvantPlayBetListDetailController(_mockBetListApiClient.Object, _mockContextService.Object);

            // Assert
            Assert.IsType<AdvantPlayBetListDetailController>(controller);
        }

        #endregion
    }
} 