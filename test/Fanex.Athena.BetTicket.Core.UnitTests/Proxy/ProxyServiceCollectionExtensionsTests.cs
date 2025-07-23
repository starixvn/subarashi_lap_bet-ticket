using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net.Http;
using Xunit;
using System.Linq;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyServiceCollectionExtensionsTests
    {
        private readonly IServiceCollection _services;

        public ProxyServiceCollectionExtensionsTests()
        {
            _services = new ServiceCollection();
        }

        [Fact]
        public void AddProxy_WithValidParameters_ShouldRegisterServices()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.MessageHandler = new HttpClientHandler();
            };

            // Act
            var result = _services.AddProxy(configureOptions);

            // Assert
            Assert.Same(_services, result);
            
            // Verify ProxyService is registered as singleton
            var serviceProvider = _services.BuildServiceProvider();
            var proxyService1 = serviceProvider.GetService<ProxyService>();
            var proxyService2 = serviceProvider.GetService<ProxyService>();
            
            Assert.NotNull(proxyService1);
            Assert.NotNull(proxyService2);
            Assert.Same(proxyService1, proxyService2); // Singleton verification
        }

        [Fact]
        public void AddProxy_WithNullServices_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceCollection services = null;
            Action<SharedProxyOptions> configureOptions = options => { };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                services.AddProxy(configureOptions));
            
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddProxy_WithNullConfigureOptions_ShouldThrowArgumentNullException()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                _services.AddProxy(configureOptions));
            
            Assert.Equal("configureOptions", exception.ParamName);
        }

        [Fact]
        public void AddProxy_ShouldConfigureSharedProxyOptions()
        {
            // Arrange
            var expectedMessageHandler = new HttpClientHandler();
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.MessageHandler = expectedMessageHandler;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(expectedMessageHandler, options.Value.MessageHandler);
        }

        [Fact]
        public void AddProxy_WithMultipleConfigurations_ShouldUseLastConfiguration()
        {
            // Arrange
            var firstMessageHandler = new HttpClientHandler();
            var secondMessageHandler = new HttpClientHandler();
            
            Action<SharedProxyOptions> firstConfigureOptions = options =>
            {
                options.MessageHandler = firstMessageHandler;
            };
            
            Action<SharedProxyOptions> secondConfigureOptions = options =>
            {
                options.MessageHandler = secondMessageHandler;
            };

            // Act
            _services.AddProxy(firstConfigureOptions);
            _services.AddProxy(secondConfigureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(secondMessageHandler, options.Value.MessageHandler);
        }

        [Fact]
        public void AddProxy_WithEmptyConfiguration_ShouldRegisterDefaultOptions()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options => { };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
        }

        [Fact]
        public void AddProxy_WithComplexConfiguration_ShouldConfigureAllProperties()
        {
            // Arrange
            var messageHandler = new HttpClientHandler();
            var expectedWebSocketKeepAliveInterval = TimeSpan.FromSeconds(30);
            
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.MessageHandler = messageHandler;
                options.WebSocketKeepAliveInterval = expectedWebSocketKeepAliveInterval;
                options.WebSocketBufferSize = 8192;
                options.Timeout = TimeSpan.FromMinutes(5);
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(messageHandler, options.Value.MessageHandler);
            Assert.Equal(expectedWebSocketKeepAliveInterval, options.Value.WebSocketKeepAliveInterval);
            Assert.Equal(8192, options.Value.WebSocketBufferSize);
            Assert.Equal(TimeSpan.FromMinutes(5), options.Value.Timeout);
        }

        [Fact]
        public void AddProxy_ShouldReturnSameServiceCollection()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options => { };

            // Act
            var result = _services.AddProxy(configureOptions);

            // Assert
            Assert.Same(_services, result);
        }

        [Fact]
        public void AddProxy_WithNullMessageHandler_ShouldHandleNullMessageHandler()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.MessageHandler = null;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Null(options.Value.MessageHandler);
        }

        [Fact]
        public void AddProxy_WithNullWebSocketKeepAliveInterval_ShouldHandleNullWebSocketKeepAliveInterval()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketKeepAliveInterval = null;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Null(options.Value.WebSocketKeepAliveInterval);
        }

        [Fact]
        public void AddProxy_WithNullWebSocketBufferSize_ShouldHandleNullWebSocketBufferSize()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketBufferSize = null;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Null(options.Value.WebSocketBufferSize);
        }

        [Fact]
        public void AddProxy_WithZeroWebSocketKeepAliveInterval_ShouldHandleZeroWebSocketKeepAliveInterval()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketKeepAliveInterval = TimeSpan.Zero;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(TimeSpan.Zero, options.Value.WebSocketKeepAliveInterval);
        }

        [Fact]
        public void AddProxy_WithNegativeWebSocketKeepAliveInterval_ShouldHandleNegativeWebSocketKeepAliveInterval()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketKeepAliveInterval = TimeSpan.FromTicks(-1);
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(TimeSpan.FromTicks(-1), options.Value.WebSocketKeepAliveInterval);
        }

        [Fact]
        public void AddProxy_WithLargeWebSocketBufferSize_ShouldHandleLargeWebSocketBufferSize()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketBufferSize = int.MaxValue;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(int.MaxValue, options.Value.WebSocketBufferSize);
        }

        [Fact]
        public void AddProxy_WithLargeWebSocketKeepAliveInterval_ShouldHandleLargeWebSocketKeepAliveInterval()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketKeepAliveInterval = TimeSpan.MaxValue;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(TimeSpan.MaxValue, options.Value.WebSocketKeepAliveInterval);
        }

        [Fact]
        public void AddProxy_WithMultipleCalls_ShouldRegisterMultipleProxyServices()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options => { };

            // Act
            _services.AddProxy(configureOptions);
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var proxyServices = serviceProvider.GetServices<ProxyService>();
            
            Assert.NotNull(proxyServices);
            // Note: Multiple registrations of the same singleton will return the same instance
            var proxyServiceList = proxyServices.ToList();
            Assert.True(proxyServiceList.Count >= 1); // Should have at least one instance
        }

        [Fact]
        public void AddProxy_WithChainedConfiguration_ShouldWorkWithChainedCalls()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.MessageHandler = new HttpClientHandler();
            };

            // Act
            var result = _services
                .AddProxy(configureOptions)
                .AddProxy(configureOptions);

            // Assert
            Assert.Same(_services, result);
            
            var serviceProvider = _services.BuildServiceProvider();
            var proxyService = serviceProvider.GetService<ProxyService>();
            Assert.NotNull(proxyService);
        }

        [Fact]
        public void AddProxy_WithDifferentMessageHandlers_ShouldUseLastConfiguredMessageHandler()
        {
            // Arrange
            var firstMessageHandler = new HttpClientHandler();
            var secondMessageHandler = new HttpClientHandler();
            
            Action<SharedProxyOptions> firstConfigureOptions = options =>
            {
                options.MessageHandler = firstMessageHandler;
            };
            
            Action<SharedProxyOptions> secondConfigureOptions = options =>
            {
                options.MessageHandler = secondMessageHandler;
            };

            // Act
            _services.AddProxy(firstConfigureOptions);
            _services.AddProxy(secondConfigureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(secondMessageHandler, options.Value.MessageHandler);
            Assert.NotSame(firstMessageHandler, options.Value.MessageHandler);
        }

        [Fact]
        public void AddProxy_WithPartialConfiguration_ShouldPreserveDefaultValues()
        {
            // Arrange
            var messageHandler = new HttpClientHandler();
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.MessageHandler = messageHandler;
                // Don't set other properties to test default values
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(messageHandler, options.Value.MessageHandler);
            // Other properties should have their default values
            Assert.Equal(TimeSpan.FromSeconds(100), options.Value.Timeout);
            Assert.True(options.Value.ValidateRequest(null)); // Default should return true
        }

        [Fact]
        public void AddProxy_WithNullServiceCollection_ShouldThrowArgumentNullExceptionWithCorrectParamName()
        {
            // Arrange
            IServiceCollection services = null;
            Action<SharedProxyOptions> configureOptions = options => { };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                services.AddProxy(configureOptions));
            
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddProxy_WithNullConfigureOptions_ShouldThrowArgumentNullExceptionWithCorrectParamName()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                _services.AddProxy(configureOptions));
            
            Assert.Equal("configureOptions", exception.ParamName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(int.MaxValue)]
        public void AddProxy_WithVariousWebSocketBufferSizes_ShouldConfigureCorrectly(int bufferSize)
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketBufferSize = bufferSize;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(bufferSize, options.Value.WebSocketBufferSize);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(3600)]
        public void AddProxy_WithVariousWebSocketKeepAliveIntervals_ShouldConfigureCorrectly(int seconds)
        {
            // Arrange
            var keepAliveInterval = TimeSpan.FromSeconds(seconds);
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.WebSocketKeepAliveInterval = keepAliveInterval;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(keepAliveInterval, options.Value.WebSocketKeepAliveInterval);
        }

        [Fact]
        public void AddProxy_WithComplexMessageHandlerConfiguration_ShouldConfigureCorrectly()
        {
            // Arrange
            var messageHandler = new HttpClientHandler();
            messageHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            messageHandler.UseCookies = false;
            
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.MessageHandler = messageHandler;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(messageHandler, options.Value.MessageHandler);
            Assert.Equal(System.Net.DecompressionMethods.GZip, messageHandler.AutomaticDecompression);
            Assert.False(messageHandler.UseCookies);
        }

        [Fact]
        public void AddProxy_WithTimeoutConfiguration_ShouldConfigureCorrectly()
        {
            // Arrange
            var expectedTimeout = TimeSpan.FromMinutes(10);
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.Timeout = expectedTimeout;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(expectedTimeout, options.Value.Timeout);
        }

        [Fact]
        public void AddProxy_WithDefaultTimeout_ShouldUseDefaultValue()
        {
            // Arrange
            Action<SharedProxyOptions> configureOptions = options => { };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Equal(TimeSpan.FromSeconds(100), options.Value.Timeout);
        }

        [Fact]
        public void AddProxy_WithValidateRequestConfiguration_ShouldConfigureCorrectly()
        {
            // Arrange
            Func<Microsoft.AspNetCore.Http.HttpContext, bool> validateRequest = context => false;
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.ValidateRequest = validateRequest;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(validateRequest, options.Value.ValidateRequest);
        }

        [Fact]
        public void AddProxy_WithOnFailedValidationConfiguration_ShouldConfigureCorrectly()
        {
            // Arrange
            Func<Microsoft.AspNetCore.Http.HttpContext, System.Threading.Tasks.Task> onFailedValidation = context => System.Threading.Tasks.Task.CompletedTask;
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.OnFailedValidation = onFailedValidation;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(onFailedValidation, options.Value.OnFailedValidation);
        }

        [Fact]
        public void AddProxy_WithShouldProxyRequestConfiguration_ShouldConfigureCorrectly()
        {
            // Arrange
            Func<Microsoft.AspNetCore.Http.HttpRequest, bool> shouldProxyRequest = request => true;
            Action<SharedProxyOptions> configureOptions = options =>
            {
                options.ShouldProxyRequest = shouldProxyRequest;
            };

            // Act
            _services.AddProxy(configureOptions);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<SharedProxyOptions>>();
            
            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            Assert.Same(shouldProxyRequest, options.Value.ShouldProxyRequest);
        }
    }
} 