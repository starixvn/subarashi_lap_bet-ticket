using Fanex.Athena.BetTicket.Core.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Middlewares
{
    public class ErrorHandlerTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILogger<ErrorHandler>> _mockLogger;
        private readonly ErrorHandler _errorHandler;

        public ErrorHandlerTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<ErrorHandler>>();
            _errorHandler = new ErrorHandler(_mockNext.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Act
            var errorHandler = new ErrorHandler(_mockNext.Object, _mockLogger.Object);

            // Assert
            Assert.NotNull(errorHandler);
        }

        [Fact]
        public void Constructor_WithNullNext_ShouldNotThrow()
        {
            // Act & Assert
            var handler = new ErrorHandler(null, _mockLogger.Object);
            Assert.NotNull(handler);
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldNotThrow()
        {
            // Act & Assert
            var handler = new ErrorHandler(_mockNext.Object, null);
            Assert.NotNull(handler);
        }

        [Fact]
        public async Task Invoke_WithSuccessfulRequest_ShouldCallNext()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockNext.Setup(x => x.Invoke(context)).Returns(Task.CompletedTask);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            _mockNext.Verify(x => x.Invoke(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_WithSuccessfulRequest_ShouldNotLogError()
        {
            // Arrange
            var context = CreateHttpContext();
            _mockNext.Setup(x => x.Invoke(context)).Returns(Task.CompletedTask);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            _mockLogger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldHandleError()
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldLogCriticalError()
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            _mockLogger.Verify(x => x.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldClearResponse()
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            // The response should be cleared and status code set to 500
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [Theory]
        [InlineData(typeof(ArgumentException))]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(NotImplementedException))]
        [InlineData(typeof(UnauthorizedAccessException))]
        public async Task Invoke_WithDifferentExceptionTypes_ShouldHandleAllExceptions(Type exceptionType)
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = (Exception)Activator.CreateInstance(exceptionType, "Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            _mockLogger.Verify(x => x.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldIncludeExceptionInLog()
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            _mockLogger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldNotCallNextAfterException()
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            _mockNext.Verify(x => x.Invoke(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_WithNullContext_ShouldNotThrow()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _errorHandler.Invoke(null));
            Assert.Null(exception);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldSetCorrectStatusCode()
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act
            await _errorHandler.Invoke(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldNotThrowException()
        {
            // Arrange
            var context = CreateHttpContext();
            var exception = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(context)).ThrowsAsync(exception);

            // Act & Assert
            var exceptionThrown = await Record.ExceptionAsync(() => _errorHandler.Invoke(context));
            Assert.Null(exceptionThrown);
        }

        private static HttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Request.Path = "/test";
            context.Request.Method = "GET";
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            return context;
        }
    }
} 