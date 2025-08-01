using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Result.Controllers;
using Fanex.Athena.BetTicket.Result.MetaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Result.UnitTests.Controllers
{
    public class SportsResultControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;

        public SportsResultControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
        }

        [Fact]
        public void SportsResultController_ShouldInheritFromController()
        {
            // Arrange & Act
            var controller = new SportsResultController();

            // Assert
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Constructor_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new SportsResultController();

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Index_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new SportsResultController();
            var methodInfo = typeof(SportsResultController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(SportsResultController);

            // Act & Assert
            var authorizeAttribute = Attribute.GetCustomAttribute(controllerType, typeof(AuthorizeAttribute));
            Assert.NotNull(authorizeAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveResponseCacheAttribute()
        {
            // Arrange
            var controllerType = typeof(SportsResultController);

            // Act & Assert
            var responseCacheAttribute = Attribute.GetCustomAttribute(controllerType, typeof(ResponseCacheAttribute));
            Assert.NotNull(responseCacheAttribute);
        }

        [Fact]
        public async Task Index_WithStatus202_ShouldReturnJsonResult()
        {
            // Arrange
            var controller = new SportsResultController();
            var metaModel = new SportsResultMetaModel
            {
                Language = "en",
                MatchId = 123456789,
                Bettype = 1
            };

            var mockResponse = new ApiResponse<string>("https://redirect-url.com", StatusCodes.Status202Accepted);

            _mockBetListApiClient.Setup(x => x.PostStringWithStatus(
                "/ResultReport/ResultPopup",
                metaModel,
                "en",
                100))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var resultValue = jsonResult.Value;

            // Use reflection to check the anonymous object properties
            var successProperty = resultValue.GetType().GetProperty("Success");
            var redirectUrlProperty = resultValue.GetType().GetProperty("RedirectUrl");

            Assert.NotNull(successProperty);
            Assert.NotNull(redirectUrlProperty);
            Assert.True((bool)successProperty.GetValue(resultValue));
            Assert.Equal("https://redirect-url.com", redirectUrlProperty.GetValue(resultValue));
        }

        [Fact]
        public async Task Index_WithNon202Status_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new SportsResultController();
            var metaModel = new SportsResultMetaModel
            {
                Language = "en",
                MatchId = 123456789,
                Bettype = 1
            };

            var mockResponse = new ApiResponse<string>("<html>Result content</html>", StatusCodes.Status200OK);

            _mockBetListApiClient.Setup(x => x.PostStringWithStatus(
                "/ResultReport/ResultPopup",
                metaModel,
                "en",
                100))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("<html>Result content</html>", controller.ViewBag.Data);
        }

        [Fact]
        public async Task Index_ShouldCallApiWithCorrectParameters()
        {
            // Arrange
            var controller = new SportsResultController();
            var metaModel = new SportsResultMetaModel
            {
                Language = "th",
                MatchId = 987654321,
                Bettype = 2
            };

            var mockResponse = new ApiResponse<string>("test content", StatusCodes.Status200OK);

            _mockBetListApiClient.Setup(x => x.PostStringWithStatus(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostStringWithStatus(
                "/ResultReport/ResultPopup",
                metaModel,
                "th",
                100),
                Times.Once);
        }

        [Fact]
        public async Task Index_WithNullMetaModel_ShouldThrowNullReferenceException()
        {
            // Arrange
            var controller = new SportsResultController();
            SportsResultMetaModel? metaModel = null;

            var mockResponse = new ApiResponse<string>("default content", StatusCodes.Status200OK);

            _mockBetListApiClient.Setup(x => x.PostStringWithStatus(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
                .ReturnsAsync(mockResponse);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() =>
                controller.Index(_mockBetListApiClient.Object, metaModel));
        }

        [Fact]
        public async Task Index_WithEmptyLanguage_ShouldPassEmptyLanguageToApi()
        {
            // Arrange
            var controller = new SportsResultController();
            var metaModel = new SportsResultMetaModel
            {
                Language = "",
                MatchId = 123456789,
                Bettype = 1
            };

            var mockResponse = new ApiResponse<string>("test content", StatusCodes.Status200OK);

            _mockBetListApiClient.Setup(x => x.PostStringWithStatus(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostStringWithStatus(
                "/ResultReport/ResultPopup",
                metaModel,
                "",
                100),
                Times.Once);
        }

        [Fact]
        public async Task Index_WithNullLanguage_ShouldPassNullLanguageToApi()
        {
            // Arrange
            var controller = new SportsResultController();
            var metaModel = new SportsResultMetaModel
            {
                Language = null,
                MatchId = 123456789,
                Bettype = 1
            };

            var mockResponse = new ApiResponse<string>("test content", StatusCodes.Status200OK);

            _mockBetListApiClient.Setup(x => x.PostStringWithStatus(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
                .ReturnsAsync(mockResponse);

            // Act
            await controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.PostStringWithStatus(
                "/ResultReport/ResultPopup",
                metaModel,
                null,
                100),
                Times.Once);
        }
    }
}