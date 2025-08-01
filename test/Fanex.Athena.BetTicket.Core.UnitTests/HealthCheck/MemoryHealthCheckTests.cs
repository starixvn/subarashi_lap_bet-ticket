using Fanex.Athena.BetTicket.Core.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.HealthCheck
{
    public class MemoryHealthCheckTests
    {
        private readonly Mock<IOptionsMonitor<MemoryCheckOptions>> _mockOptionsMonitor;
        private readonly Mock<IOptions<MemoryCheckOptions>> _mockOptions;
        private readonly MemoryHealthCheck _memoryHealthCheck;
        private readonly MemoryCheckOptions _options;

        public MemoryHealthCheckTests()
        {
            _mockOptionsMonitor = new Mock<IOptionsMonitor<MemoryCheckOptions>>();
            _mockOptions = new Mock<IOptions<MemoryCheckOptions>>();
            _options = new MemoryCheckOptions { Threshold = 1024L }; // 1GB default
            _mockOptionsMonitor.Setup(x => x.Get(It.IsAny<string>())).Returns(_options);
            _memoryHealthCheck = new MemoryHealthCheck(_mockOptionsMonitor.Object);
        }

        [Fact]
        public void Constructor_WithValidOptions_ShouldInitialize()
        {
            // Act
            var healthCheck = new MemoryHealthCheck(_mockOptionsMonitor.Object);

            // Assert
            Assert.NotNull(healthCheck);
        }

        [Fact]
        public void Constructor_WithNullOptions_ShouldInitialize()
        {
            // Act & Assert
            // Note: The constructor doesn't validate null parameters
            var healthCheck = new MemoryHealthCheck(null);
            Assert.NotNull(healthCheck);
        }

        [Fact]
        public async Task CheckHealthAsync_WithLowMemoryUsage_ShouldReturnHealthy()
        {
            // Arrange
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _memoryHealthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _memoryHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Contains("Reports degraded status", result.Description);
            Assert.NotNull(result.Data);
            Assert.Contains("AllocatedMBs", result.Data.Keys);
            Assert.Contains("Gen0Collections", result.Data.Keys);
            Assert.Contains("Gen1Collections", result.Data.Keys);
            Assert.Contains("Gen2Collections", result.Data.Keys);
        }

        [Fact]
        public async Task CheckHealthAsync_WithHighMemoryUsage_ShouldReturnUnhealthy()
        {
            // Arrange
            var highThresholdOptions = new MemoryCheckOptions { Threshold = 1L }; // Very low threshold
            _mockOptionsMonitor.Setup(x => x.Get(It.IsAny<string>())).Returns(highThresholdOptions);
            var healthCheck = new MemoryHealthCheck(_mockOptionsMonitor.Object);
            
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await healthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(HealthStatus.Unhealthy, result.Status);
            Assert.Contains("Reports degraded status", result.Description);
        }

        [Fact]
        public async Task CheckHealthAsync_WithCustomFailureStatus_ShouldReturnCustomStatus()
        {
            // Arrange
            var highThresholdOptions = new MemoryCheckOptions { Threshold = 1L }; // Very low threshold
            _mockOptionsMonitor.Setup(x => x.Get(It.IsAny<string>())).Returns(highThresholdOptions);
            var healthCheck = new MemoryHealthCheck(_mockOptionsMonitor.Object);
            
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", healthCheck, HealthStatus.Degraded, new string[0])
            };

            // Act
            var result = await healthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(HealthStatus.Degraded, result.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnCorrectDataStructure()
        {
            // Arrange
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _memoryHealthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _memoryHealthCheck.CheckHealthAsync(context);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(4, result.Data.Count);
            
            // Verify data types
            Assert.IsType<long>(result.Data["AllocatedMBs"]);
            Assert.IsType<int>(result.Data["Gen0Collections"]);
            Assert.IsType<int>(result.Data["Gen1Collections"]);
            Assert.IsType<int>(result.Data["Gen2Collections"]);
        }

        [Fact]
        public async Task CheckHealthAsync_WithCancellationToken_ShouldHandleCancellation()
        {
            // Arrange
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _memoryHealthCheck, HealthStatus.Unhealthy, new string[0])
            };
            var cancellationToken = new CancellationToken();

            // Act
            var result = await _memoryHealthCheck.CheckHealthAsync(context, cancellationToken);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_WithDifferentThresholds_ShouldRespectThreshold()
        {
            // Arrange
            var lowThresholdOptions = new MemoryCheckOptions { Threshold = 10000L }; // 10GB
            _mockOptionsMonitor.Setup(x => x.Get(It.IsAny<string>())).Returns(lowThresholdOptions);
            var healthCheck = new MemoryHealthCheck(_mockOptionsMonitor.Object);
            
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await healthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public void MemoryCheckOptions_DefaultThreshold_ShouldBe1024()
        {
            // Act
            var options = new MemoryCheckOptions();

            // Assert
            Assert.Equal(1024L, options.Threshold);
        }

        [Fact]
        public void MemoryCheckOptions_CanSetThreshold()
        {
            // Arrange
            var options = new MemoryCheckOptions();

            // Act
            options.Threshold = 2048L;

            // Assert
            Assert.Equal(2048L, options.Threshold);
        }

        [Theory]
        [InlineData(512L, HealthStatus.Healthy)]
        [InlineData(1024L, HealthStatus.Healthy)]
        [InlineData(1L, HealthStatus.Unhealthy)]
        public async Task CheckHealthAsync_WithVariousThresholds_ShouldReturnExpectedStatus(long threshold, HealthStatus expectedStatus)
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = threshold };
            _mockOptionsMonitor.Setup(x => x.Get(It.IsAny<string>())).Returns(options);
            var healthCheck = new MemoryHealthCheck(_mockOptionsMonitor.Object);
            
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await healthCheck.CheckHealthAsync(context);

            // Assert
            Assert.Equal(expectedStatus, result.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldUseCorrectRegistrationName()
        {
            // Arrange
            var expectedName = "custom-memory-check";
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration(expectedName, _memoryHealthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            await _memoryHealthCheck.CheckHealthAsync(context);

            // Assert
            _mockOptionsMonitor.Verify(x => x.Get(expectedName), Times.Once);
        }
    }
} 