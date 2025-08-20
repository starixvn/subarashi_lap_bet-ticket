using Fanex.Athena.BetTicket.Core.Middlewares;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using ONEbook.UM.Services;
using ONEbook.UM.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Middlewares
{
    public class UnderMaintenanceHandlerTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<IMaintenanceService> _mockMaintenanceService;
        private readonly UnderMaintenanceHandler _underMaintenanceHandler;

        public UnderMaintenanceHandlerTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockMaintenanceService = new Mock<IMaintenanceService>();
            _underMaintenanceHandler = new UnderMaintenanceHandler(_mockNext.Object, _mockMaintenanceService.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Act
            var handler = new UnderMaintenanceHandler(_mockNext.Object, _mockMaintenanceService.Object);

            // Assert
            Assert.NotNull(handler);
        }

        [Fact]
        public void Constructor_WithNullNext_ShouldNotThrow()
        {
            // Act & Assert
            var handler = new UnderMaintenanceHandler(null, _mockMaintenanceService.Object);
            Assert.NotNull(handler);
        }

        [Fact]
        public void Constructor_WithNullMaintenanceService_ShouldNotThrow()
        {
            // Act & Assert
            var handler = new UnderMaintenanceHandler(_mockNext.Object, null);
            Assert.NotNull(handler);
        }

        [Fact]
        public async Task Invoke_WhenNotUnderMaintenance_ShouldCallNext()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(false);

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            _mockNext.Verify(x => x.Invoke(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenNotUnderMaintenance_ShouldNotWriteResponse()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(false);

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            Assert.Equal(0, context.Response.Body.Length);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenance_ShouldNotCallNext()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                .Returns(Task.FromResult(new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" }));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            _mockNext.Verify(x => x.Invoke(context), Times.Never);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenanceWithAjaxRequest_ShouldReturnJsonResponse()
        {
            // Arrange
            var context = CreateHttpContext();
            context.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                .Returns(Task.FromResult(new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" }));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            Assert.Equal("application/json", context.Response.ContentType);
            context.Response.Body.Position = 0;
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<dynamic>(responseBody);
            Assert.False((bool)response.isSuccess);
            Assert.Contains("System is currently unavailable due to scheduled maintenance", response.msg.ToString());
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenanceWithHtmlRequest_ShouldReturnHtmlResponse()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                .Returns(Task.FromResult(new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" }));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            Assert.Equal("text/html", context.Response.ContentType);
            context.Response.Body.Position = 0;
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("System is currently unavailable due to scheduled maintenance", responseBody);
            Assert.Contains("2024-01-01 10:00", responseBody);
            Assert.Contains("2024-01-01 12:00", responseBody);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenance_ShouldIncludeMaintenanceTimes()
        {
            // Arrange
            var context = CreateHttpContext();
            var maintenanceTime = new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" };
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync()).Returns(Task.FromResult(maintenanceTime));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            context.Response.Body.Position = 0;
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains(maintenanceTime.From, responseBody);
            Assert.Contains(maintenanceTime.To, responseBody);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenance_ShouldCallMaintenanceService()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                .Returns(Task.FromResult(new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" }));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            _mockMaintenanceService.Verify(x => x.IsUnderMaintenanceAsync(), Times.Once);
            _mockMaintenanceService.Verify(x => x.GetUnderMaintenanceTimeAsync(), Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenNotUnderMaintenance_ShouldNotCallGetMaintenanceTime()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(false);

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            _mockMaintenanceService.Verify(x => x.IsUnderMaintenanceAsync(), Times.Once);
            _mockMaintenanceService.Verify(x => x.GetUnderMaintenanceTimeAsync(), Times.Never);
        }

        [Theory]
        [InlineData("XMLHttpRequest", "application/json")]
        [InlineData("xmlhttprequest", "application/json")]
        [InlineData("", "text/html")]
        [InlineData(null, "text/html")]
        public async Task Invoke_WithDifferentRequestTypes_ShouldReturnCorrectContentType(string headerValue, string expectedContentType)
        {
            // Arrange
            var context = CreateHttpContext();
            if (!string.IsNullOrEmpty(headerValue))
            {
                context.Request.Headers["X-Requested-With"] = headerValue;
            }
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                .Returns(Task.FromResult(new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" }));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            Assert.Equal(expectedContentType, context.Response.ContentType);
        }

        [Fact]
        public async Task Invoke_WithAjaxRequest_ShouldReturnJsonWithCorrectStructure()
        {
            // Arrange
            var context = CreateHttpContext();
            context.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                .Returns(Task.FromResult(new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" }));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            context.Response.Body.Position = 0;
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<dynamic>(responseBody);
            Assert.NotNull(response.msg);
            Assert.False((bool)response.isSuccess);
        }

        [Fact]
        public async Task Invoke_WithHtmlRequest_ShouldReturnHtmlWithCorrectStructure()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                .Returns(Task.FromResult(new UnderMaintenanceTime { From = "2024-01-01 10:00", To = "2024-01-01 12:00" }));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            context.Response.Body.Position = 0;
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("<div", responseBody);
            Assert.Contains("<b>", responseBody);
            Assert.Contains("</b>", responseBody);
        }

        [Fact]
        public async Task Invoke_WithNullContext_ShouldNotThrow()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _underMaintenanceHandler.Invoke(null));
            Assert.Null(exception);
        }

        [Fact]
        public async Task Invoke_WhenMaintenanceServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ThrowsAsync(new InvalidOperationException("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _underMaintenanceHandler.Invoke(context));
        }

        [Fact]
        public async Task Invoke_WhenGetMaintenanceTimeThrowsException_ShouldPropagateException()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync()).ThrowsAsync(new InvalidOperationException("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _underMaintenanceHandler.Invoke(context));
        }

        [Fact]
        public async Task Invoke_WithDifferentMaintenanceTimes_ShouldIncludeCorrectTimes()
        {
            // Arrange
            var context = CreateHttpContext();
            var maintenanceTime = new UnderMaintenanceTime { From = "2024-12-25 23:59", To = "2024-12-26 06:00" };
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync()).Returns(Task.FromResult(maintenanceTime));

            // Act
            await _underMaintenanceHandler.Invoke(context);

            // Assert
            context.Response.Body.Position = 0;
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains(maintenanceTime.From, responseBody);
            Assert.Contains(maintenanceTime.To, responseBody);
        }

        private static HttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Request.Path = "/test";
            context.Request.Method = "GET";
            return context;
        }
    }
} 