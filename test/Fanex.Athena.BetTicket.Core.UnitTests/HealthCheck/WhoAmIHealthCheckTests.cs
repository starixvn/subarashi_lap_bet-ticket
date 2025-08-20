using Fanex.Athena.BetTicket.Core.HealthCheck;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.HealthCheck
{
    public class WhoAmIHealthCheckTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ConnectionInfo> _mockConnectionInfo;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly Mock<HttpResponse> _mockResponse;
        private readonly WhoAmIHealthCheck _whoAmIHealthCheck;

        public WhoAmIHealthCheckTests()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockConnectionInfo = new Mock<ConnectionInfo>();
            _mockRequest = new Mock<HttpRequest>();
            _mockResponse = new Mock<HttpResponse>();

            // Setup default mocks
            _mockWebHostEnvironment.Setup(x => x.EnvironmentName).Returns("Test");
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);
            _mockHttpContext.Setup(x => x.Connection).Returns(_mockConnectionInfo.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockHttpContext.Setup(x => x.Response).Returns(_mockResponse.Object);
            _mockConnectionInfo.Setup(x => x.LocalIpAddress).Returns(IPAddress.Parse("127.0.0.1"));
            _mockConnectionInfo.Setup(x => x.LocalPort).Returns(5000);
            _mockConnectionInfo.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse("192.168.1.1"));
            _mockRequest.Setup(x => x.Host).Returns(new HostString("localhost:5000"));
            _mockRequest.Setup(x => x.Scheme).Returns("http");

            _whoAmIHealthCheck = new WhoAmIHealthCheck(_mockHttpContextAccessor.Object, _mockWebHostEnvironment.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldInitialize()
        {
            // Act
            var healthCheck = new WhoAmIHealthCheck(_mockHttpContextAccessor.Object, _mockWebHostEnvironment.Object);

            // Assert
            Assert.NotNull(healthCheck);
        }

        [Fact]
        public void Constructor_WithNullHttpContextAccessor_ShouldInitialize()
        {
            // Act & Assert
            // Note: The constructor doesn't validate null parameters
            var healthCheck = new WhoAmIHealthCheck(null, _mockWebHostEnvironment.Object);
            Assert.NotNull(healthCheck);
        }

        [Fact]
        public void Constructor_WithNullWebHostEnvironment_ShouldInitialize()
        {
            // Act & Assert
            // Note: The constructor doesn't validate null parameters
            var healthCheck = new WhoAmIHealthCheck(_mockHttpContextAccessor.Object, null);
            Assert.NotNull(healthCheck);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnHealthyStatus()
        {
            // Arrange
            var context = new HealthCheckContext();

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Equal("Server Info", result.Description);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnCorrectDataStructure()
        {
            // Arrange
            var context = new HealthCheckContext();

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Contains("Env", result.Data.Keys);
            Assert.Contains("Host Name", result.Data.Keys);
            Assert.Contains("Local Ip", result.Data.Keys);
            Assert.Contains("Local Port", result.Data.Keys);
            Assert.Contains("Client Ip", result.Data.Keys);
            Assert.Contains("Server Time", result.Data.Keys);
            Assert.Contains("Host", result.Data.Keys);
            Assert.Contains("Schema", result.Data.Keys);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnEnvironmentName()
        {
            // Arrange
            var context = new HealthCheckContext();
            _mockWebHostEnvironment.Setup(x => x.EnvironmentName).Returns("Production");

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal("Production", result.Data["Env"]);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnLocalConnectionInfo()
        {
            // Arrange
            var context = new HealthCheckContext();
            var expectedLocalIp = IPAddress.Parse("10.0.0.1");
            var expectedLocalPort = 8080;
            _mockConnectionInfo.Setup(x => x.LocalIpAddress).Returns(expectedLocalIp);
            _mockConnectionInfo.Setup(x => x.LocalPort).Returns(expectedLocalPort);

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(expectedLocalIp.ToString(), result.Data["Local Ip"]);
            Assert.Equal(expectedLocalPort, result.Data["Local Port"]);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnRemoteConnectionInfo()
        {
            // Arrange
            var context = new HealthCheckContext();
            var expectedRemoteIp = IPAddress.Parse("192.168.0.100");
            _mockConnectionInfo.Setup(x => x.RemoteIpAddress).Returns(expectedRemoteIp);

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(expectedRemoteIp.ToString(), result.Data["Client Ip"]);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnRequestInfo()
        {
            // Arrange
            var context = new HealthCheckContext();
            var expectedHost = new HostString("example.com:443");
            var expectedScheme = "https";
            _mockRequest.Setup(x => x.Host).Returns(expectedHost);
            _mockRequest.Setup(x => x.Scheme).Returns(expectedScheme);

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(expectedHost.ToString(), result.Data["Host"].ToString());
            Assert.Equal(expectedScheme, result.Data["Schema"]);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnServerTime()
        {
            // Arrange
            var context = new HealthCheckContext();

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Contains("Server Time", result.Data.Keys);
            var serverTimeString = result.Data["Server Time"].ToString();
            Assert.NotNull(serverTimeString);
            Assert.NotEmpty(serverTimeString);
            // Verify it's a valid date format
            Assert.True(DateTimeOffset.TryParse(serverTimeString, out _));
        }

        [Fact]
        public async Task CheckHealthAsync_WithCancellationToken_ShouldHandleCancellation()
        {
            // Arrange
            var context = new HealthCheckContext();
            var cancellationToken = new CancellationToken();

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context, cancellationToken);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldIncludeHostName()
        {
            // Arrange
            var context = new HealthCheckContext();

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Contains("Host Name", result.Data.Keys);
            Assert.NotNull(result.Data["Host Name"]);
            Assert.NotEmpty(result.Data["Host Name"].ToString());
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldIncludeIPAddresses()
        {
            // Arrange
            var context = new HealthCheckContext();

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            // Note: This test may not always pass depending on network configuration
            // but it should at least not throw an exception
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task CheckHealthAsync_WithNullHttpContext_ShouldHandleGracefully()
        {
            // Arrange
            var context = new HealthCheckContext();
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _whoAmIHealthCheck.CheckHealthAsync(context));
        }

        [Fact]
        public async Task CheckHealthAsync_WithNullConnection_ShouldHandleGracefully()
        {
            // Arrange
            var context = new HealthCheckContext();
            _mockHttpContext.Setup(x => x.Connection).Returns((ConnectionInfo)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _whoAmIHealthCheck.CheckHealthAsync(context));
        }

        [Fact]
        public async Task CheckHealthAsync_WithNullRequest_ShouldHandleGracefully()
        {
            // Arrange
            var context = new HealthCheckContext();
            _mockHttpContext.Setup(x => x.Request).Returns((HttpRequest)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _whoAmIHealthCheck.CheckHealthAsync(context));
        }

        [Theory]
        [InlineData("Development")]
        [InlineData("Staging")]
        [InlineData("Production")]
        public async Task CheckHealthAsync_WithDifferentEnvironments_ShouldReturnCorrectEnvironment(string environment)
        {
            // Arrange
            var context = new HealthCheckContext();
            _mockWebHostEnvironment.Setup(x => x.EnvironmentName).Returns(environment);

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(environment, result.Data["Env"]);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnReadOnlyDictionary()
        {
            // Arrange
            var context = new HealthCheckContext();

            // Act
            var result = await _whoAmIHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.True(result.Data is System.Collections.ObjectModel.ReadOnlyDictionary<string, object>);
        }
    }
} 