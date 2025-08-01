using Fanex.Athena.BetTicket.Result.Controllers;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Result.UnitTests.Controllers
{
    public class NonSportsResultControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;

        public NonSportsResultControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
        }

        [Fact]
        public void NonSportsResultController_ShouldInheritFromController()
        {
            // Arrange & Act
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);

            // Assert
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            var controller = new NonSportsResultController(null);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void SAGaming_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var methodInfo = typeof(NonSportsResultController).GetMethod("SAGaming");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void AllBet_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var methodInfo = typeof(NonSportsResultController).GetMethod("AllBet");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void WM_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var methodInfo = typeof(NonSportsResultController).GetMethod("WM");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void WCasino_ShouldNotHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var methodInfo = typeof(NonSportsResultController).GetMethod("WCasino");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.Null(httpGetAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(NonSportsResultController);

            // Act & Assert
            var authorizeAttribute = Attribute.GetCustomAttribute(controllerType, typeof(AuthorizeAttribute));
            Assert.NotNull(authorizeAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveResponseCacheAttribute()
        {
            // Arrange
            var controllerType = typeof(NonSportsResultController);

            // Act & Assert
            var responseCacheAttribute = Attribute.GetCustomAttribute(controllerType, typeof(ResponseCacheAttribute));
            Assert.NotNull(responseCacheAttribute);
        }

        [Fact]
        public async Task SAGaming_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var matchCode = "test-match";
            var betType = "test-bet";
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            var result = await controller.SAGaming(transDesc, matchCode, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task SAGaming_ShouldSetViewBagData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var matchCode = "test-match";
            var betType = "test-bet";
            var language = "en";
            var mockResponse = "<html>{rootUrl}test data</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.SAGaming(transDesc, matchCode, betType, language);

            // Assert
            Assert.NotNull(controller.ViewBag.Data);
            Assert.Equal("<html>test data</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task SAGaming_ShouldCallCorrectApiPath()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var matchCode = "test-match";
            var betType = "test-bet";
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            await controller.SAGaming(transDesc, matchCode, betType, language);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostString("/SAGamingResultPopup/Index", It.IsAny<object>(), language, 100), Times.Once);
        }

        [Fact]
        public async Task AllBet_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var betCheck = "test-check";
            var betType = "test-bet";
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            var result = await controller.AllBet(transDesc, betCheck, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AllBet_ShouldSetViewBagData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var betCheck = "test-check";
            var betType = "test-bet";
            var language = "en";
            var mockResponse = "<html>{rootUrl}test data</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.AllBet(transDesc, betCheck, betType, language);

            // Assert
            Assert.NotNull(controller.ViewBag.Data);
            Assert.Equal("<html>test data</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task AllBet_ShouldCallCorrectApiPath()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var betCheck = "test-check";
            var betType = "test-bet";
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            await controller.AllBet(transDesc, betCheck, betType, language);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostString("/AllBetResultPopup/Index", It.IsAny<object>(), language, 100), Times.Once);
        }

        [Fact]
        public async Task WM_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            var result = await controller.WM(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task WM_ShouldSetViewBagData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";
            var mockResponse = "<html>{rootUrl}test data</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.WM(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.NotNull(controller.ViewBag.Data);
            Assert.Equal("<html>test data</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task WM_ShouldCallCorrectApiPath()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            await controller.WM(transDesc, gameId3rd, betType, language);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostString("/WMResultPopup/Index", It.IsAny<object>(), language, 100), Times.Once);
        }

        [Fact]
        public async Task WCasino_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            var result = await controller.WCasino(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task WCasino_ShouldSetViewBagData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";
            var mockResponse = "<html>{rootUrl}test data</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.WCasino(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.NotNull(controller.ViewBag.Data);
            Assert.Equal("<html>test data</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task WCasino_ShouldCallCorrectApiPath()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act
            await controller.WCasino(transDesc, gameId3rd, betType, language);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostString("/WCasinoResultPopup/Index", It.IsAny<object>(), language, 100), Times.Once);
        }

        [Fact]
        public async Task SAGaming_WithNullParameters_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act & Assert
            var result = await controller.SAGaming(null, null, null, null);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AllBet_WithNullParameters_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act & Assert
            var result = await controller.AllBet(null, null, null, null);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task WM_WithNullParameters_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act & Assert
            var result = await controller.WM(null, null, 0, null);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task WCasino_WithNullParameters_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("<html>test data</html>");

            // Act & Assert
            var result = await controller.WCasino(null, null, 0, null);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task SAGaming_WithEmptyResponse_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var matchCode = "test-match";
            var betType = "test-bet";
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("");

            // Act
            var result = await controller.SAGaming(transDesc, matchCode, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("", controller.ViewBag.Data);
        }

        [Fact]
        public async Task AllBet_WithEmptyResponse_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var betCheck = "test-check";
            var betType = "test-bet";
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("");

            // Act
            var result = await controller.AllBet(transDesc, betCheck, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("", controller.ViewBag.Data);
        }

        [Fact]
        public async Task WM_WithEmptyResponse_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("");

            // Act
            var result = await controller.WM(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("", controller.ViewBag.Data);
        }

        [Fact]
        public async Task WCasino_WithEmptyResponse_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync("");

            // Act
            var result = await controller.WCasino(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("", controller.ViewBag.Data);
        }

        [Fact]
        public async Task SAGaming_WithMultipleRootUrlReplacements_ShouldReplaceAll()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var matchCode = "test-match";
            var betType = "test-bet";
            var language = "en";
            var mockResponse = "<html>{rootUrl}test{rootUrl}data{rootUrl}</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.SAGaming(transDesc, matchCode, betType, language);

            // Assert
            Assert.Equal("<html>testdata</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task AllBet_WithMultipleRootUrlReplacements_ShouldReplaceAll()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var betCheck = "test-check";
            var betType = "test-bet";
            var language = "en";
            var mockResponse = "<html>{rootUrl}test{rootUrl}data{rootUrl}</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.AllBet(transDesc, betCheck, betType, language);

            // Assert
            Assert.Equal("<html>testdata</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task WM_WithMultipleRootUrlReplacements_ShouldReplaceAll()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";
            var mockResponse = "<html>{rootUrl}test{rootUrl}data{rootUrl}</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.WM(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.Equal("<html>testdata</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task WCasino_WithMultipleRootUrlReplacements_ShouldReplaceAll()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";
            var mockResponse = "<html>{rootUrl}test{rootUrl}data{rootUrl}</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.WCasino(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.Equal("<html>testdata</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task SAGaming_WithNoRootUrlInResponse_ShouldNotModifyData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var matchCode = "test-match";
            var betType = "test-bet";
            var language = "en";
            var mockResponse = "<html>test data without rootUrl</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.SAGaming(transDesc, matchCode, betType, language);

            // Assert
            Assert.Equal(mockResponse, controller.ViewBag.Data);
        }

        [Fact]
        public async Task AllBet_WithNoRootUrlInResponse_ShouldNotModifyData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var betCheck = "test-check";
            var betType = "test-bet";
            var language = "en";
            var mockResponse = "<html>test data without rootUrl</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.AllBet(transDesc, betCheck, betType, language);

            // Assert
            Assert.Equal(mockResponse, controller.ViewBag.Data);
        }

        [Fact]
        public async Task WM_WithNoRootUrlInResponse_ShouldNotModifyData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";
            var mockResponse = "<html>test data without rootUrl</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.WM(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.Equal(mockResponse, controller.ViewBag.Data);
        }

        [Fact]
        public async Task WCasino_WithNoRootUrlInResponse_ShouldNotModifyData()
        {
            // Arrange
            var controller = new NonSportsResultController(_mockBetListApiClient.Object);
            var transDesc = "test-trans";
            var gameId3rd = "test-game";
            var betType = 1;
            var language = "en";
            var mockResponse = "<html>test data without rootUrl</html>";

            _mockBetListApiClient.Setup(x => x.PostString(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), 100))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.WCasino(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.Equal(mockResponse, controller.ViewBag.Data);
        }
    }
} 