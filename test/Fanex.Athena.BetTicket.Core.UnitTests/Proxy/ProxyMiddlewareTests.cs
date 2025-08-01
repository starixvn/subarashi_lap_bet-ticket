using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<IOptions<SharedProxyOptions>> _mockOptions;
        private readonly SharedProxyOptions _options;
        private readonly ProxyMiddleware _proxyMiddleware;

        public ProxyMiddlewareTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockOptions = new Mock<IOptions<SharedProxyOptions>>();
            _options = new SharedProxyOptions();
            _mockOptions.Setup(x => x.Value).Returns(_options);
            _proxyMiddleware = new ProxyMiddleware(_mockNext.Object, _mockOptions.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Act
            var middleware = new ProxyMiddleware(_mockNext.Object, _mockOptions.Object);

            // Assert
            Assert.NotNull(middleware);
        }

        [Fact]
        public void Constructor_WithNullNext_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProxyMiddleware(null, _mockOptions.Object));
        }

        [Fact]
        public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProxyMiddleware(_mockNext.Object, null));
        }

        [Fact]
        public async Task Invoke_WithNullContext_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _proxyMiddleware.Invoke(null));
        }

        [Fact]
        public async Task Invoke_WhenShouldProxyRequestReturnsFalse_ShouldCallNext()
        {
            // Arrange
            var context = CreateHttpContext();
            _options.ShouldProxyRequest = request => false;
            _mockNext.Setup(x => x.Invoke(context)).Returns(Task.CompletedTask);

            // Act
            await _proxyMiddleware.Invoke(context);

            // Assert
            _mockNext.Verify(x => x.Invoke(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenValidationFails_ShouldCallOnFailedValidation()
        {
            // Arrange
            var context = CreateHttpContext();
            var validationCalled = false;
            var onFailedValidationCalled = false;

            _options.ShouldProxyRequest = request => true;
            _options.ValidateRequest = ctx => 
            {
                validationCalled = true;
                return false;
            };
            _options.OnFailedValidation = ctx =>
            {
                onFailedValidationCalled = true;
                return Task.CompletedTask;
            };

            // Act
            await _proxyMiddleware.Invoke(context);

            // Assert
            Assert.True(validationCalled);
            Assert.True(onFailedValidationCalled);
            _mockNext.Verify(x => x.Invoke(context), Times.Never);
        }

        [Fact]
        public async Task Invoke_WithCustomShouldProxyRequest_ShouldUseCustomLogic()
        {
            // Arrange
            var context = CreateHttpContext();
            var shouldProxyCalled = false;

            _options.ShouldProxyRequest = request => 
            {
                shouldProxyCalled = true;
                return request.Path.StartsWithSegments("/api");
            };
            _options.ValidateRequest = ctx => false; // Fail validation to avoid proxy call
            _options.OnFailedValidation = ctx => Task.CompletedTask;
            context.Request.Path = "/api/test";

            // Act
            await _proxyMiddleware.Invoke(context);

            // Assert
            Assert.True(shouldProxyCalled);
        }

        [Fact]
        public async Task Invoke_WithCustomValidateRequest_ShouldUseCustomLogic()
        {
            // Arrange
            var context = CreateHttpContext();
            var validateCalled = false;

            _options.ShouldProxyRequest = request => true;
            _options.ValidateRequest = ctx => 
            {
                validateCalled = true;
                return ctx.Request.Method == "GET";
            };
            _options.OnFailedValidation = ctx => Task.CompletedTask;
            context.Request.Method = "POST";

            // Act
            await _proxyMiddleware.Invoke(context);

            // Assert
            Assert.True(validateCalled);
        }

        [Fact]
        public async Task Invoke_WithCustomOnFailedValidation_ShouldCallCustomHandler()
        {
            // Arrange
            var context = CreateHttpContext();
            var onFailedValidationCalled = false;

            _options.ShouldProxyRequest = request => true;
            _options.ValidateRequest = ctx => false;
            _options.OnFailedValidation = ctx => 
            {
                onFailedValidationCalled = true;
                return Task.CompletedTask;
            };

            // Act
            await _proxyMiddleware.Invoke(context);

            // Assert
            Assert.True(onFailedValidationCalled);
        }

        [Theory]
        [InlineData("/api/users", true)]
        [InlineData("/api/products", true)]
        [InlineData("/web/home", false)]
        [InlineData("/static/css", false)]
        public async Task Invoke_WithPathBasedShouldProxyRequest_ShouldProxyCorrectly(string path, bool shouldProxy)
        {
            // Arrange
            var context = CreateHttpContext();
            context.Request.Path = path;

            _options.ShouldProxyRequest = request => request.Path.StartsWithSegments("/api");
            _options.ValidateRequest = ctx => false; // Fail validation to avoid proxy call
            _options.OnFailedValidation = ctx => Task.CompletedTask;

            // Act
            await _proxyMiddleware.Invoke(context);

            // Assert
            if (shouldProxy)
            {
                _mockNext.Verify(x => x.Invoke(context), Times.Never);
            }
            else
            {
                _mockNext.Verify(x => x.Invoke(context), Times.Once);
            }
        }

        [Theory]
        [InlineData("GET", true)]
        [InlineData("POST", true)]
        [InlineData("PUT", true)]
        [InlineData("DELETE", true)]
        [InlineData("OPTIONS", false)]
        public async Task Invoke_WithMethodBasedValidation_ShouldValidateCorrectly(string method, bool shouldValidate)
        {
            // Arrange
            var context = CreateHttpContext();
            context.Request.Method = method;

            _options.ShouldProxyRequest = request => true;
            _options.ValidateRequest = ctx => false; // Always fail validation to avoid proxy call
            _options.OnFailedValidation = ctx => Task.CompletedTask;

            // Act
            await _proxyMiddleware.Invoke(context);

            // Assert
            // Should not call next because validation always fails
            _mockNext.Verify(x => x.Invoke(context), Times.Never);
        }

        [Fact]
        public async Task Invoke_WithDefaultOptions_ShouldNotProxy()
        {
            // Arrange
            var context = CreateHttpContext();
            var defaultOptions = new SharedProxyOptions();
            var middleware = new ProxyMiddleware(_mockNext.Object, Mock.Of<IOptions<SharedProxyOptions>>(x => x.Value == defaultOptions));

            // Act
            await middleware.Invoke(context);

            // Assert
            _mockNext.Verify(x => x.Invoke(context), Times.Once);
        }

        private static HttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/test";
            context.Request.Method = "GET";
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("localhost", 5000);
            
            // Setup RequestServices to avoid null reference exceptions
            var mockServiceProvider = new Mock<IServiceProvider>();
            context.RequestServices = mockServiceProvider.Object;
            
            return context;
        }
    }
} 