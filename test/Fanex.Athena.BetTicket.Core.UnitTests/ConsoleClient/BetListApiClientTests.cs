using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.ConsoleClient
{
    public class BetListApiClientTests
    {
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly BetListApiClient _betListApiClient;

        public BetListApiClientTests()
        {
            _mockContextService = new Mock<IContextService>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com/")
            };
            _betListApiClient = new BetListApiClient(_httpClient, _mockContextService.Object);

            // Setup default context service behavior
            _mockContextService.Setup(x => x.SiteDBServer).Returns("test-server");
            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync("test-token");
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var client = new BetListApiClient(_httpClient, _mockContextService.Object);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void Constructor_WithNullHttpClient_ShouldInitializeCorrectly()
        {
            // Act & Assert
            var client = new BetListApiClient(null, _mockContextService.Object);
            Assert.NotNull(client);
        }

        [Fact]
        public void Constructor_WithNullContextService_ShouldInitializeCorrectly()
        {
            // Act & Assert
            var client = new BetListApiClient(_httpClient, null);
            Assert.NotNull(client);
        }

        [Fact]
        public async Task PostString_WithValidRequest_ShouldReturnResponseContent()
        {
            // Arrange
            var expectedResponse = "{\"success\": true, \"data\": \"test data\"}";
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _betListApiClient.PostString("/api/test", requestData, language);

            // Assert
            Assert.Equal(expectedResponse, result);
            VerifyHttpRequestHeaders();
        }

        [Fact]
        public async Task PostString_WithTimeout_ShouldUseSpecifiedTimeout()
        {
            // Arrange
            var expectedResponse = "{\"success\": true}";
            var requestData = new { test = "value" };
            var language = "en-US";
            var timeout = 200;

            SetupMockHttpResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _betListApiClient.PostString("/api/test", requestData, language, timeout);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task PostString_WithHttpError_ShouldThrowServiceException()
        {
            // Arrange
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.BadRequest, "Bad Request");

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => 
                _betListApiClient.PostString("/api/test", requestData, language));
        }

        [Fact]
        public async Task PostStream_WithValidRequest_ShouldReturnStream()
        {
            // Arrange
            var expectedContent = "{\"success\": true, \"data\": \"test data\"}";
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.OK, expectedContent);

            // Act
            using var result = await _betListApiClient.PostStream("/api/test", requestData, language);

            // Assert
            Assert.NotNull(result);
            using var reader = new StreamReader(result);
            var content = await reader.ReadToEndAsync();
            Assert.Equal(expectedContent, content);
        }

        [Fact]
        public async Task PostStream_WithNoContentResponse_ShouldReturnEmptyStream()
        {
            // Arrange
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.NoContent, "");

            // Act
            using var result = await _betListApiClient.PostStream("/api/test", requestData, language);

            // Assert
            Assert.NotNull(result);
            using var reader = new StreamReader(result);
            var content = await reader.ReadToEndAsync();
            Assert.Equal("", content);
        }

        [Fact]
        public async Task PostStringWithStatus_WithValidRequest_ShouldReturnApiResponse()
        {
            // Arrange
            var expectedResponse = "{\"success\": true, \"data\": \"test data\"}";
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _betListApiClient.PostStringWithStatus("/api/test", requestData, language);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse, result.Content);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PostStringWithStatus_WithHttpError_ShouldReturnErrorResponse()
        {
            // Arrange
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.BadRequest, "Bad Request");

            // Act
            var result = await _betListApiClient.PostStringWithStatus("/api/test", requestData, language);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Bad Request", result.Content);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task PostStringWithStatus_WithNetworkError_ShouldThrowServiceException()
        {
            // Arrange
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponseWithException(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => 
                _betListApiClient.PostStringWithStatus("/api/test", requestData, language));
        }

        [Fact]
        public async Task PostStringWithStatus_WithTimeout_ShouldUseSpecifiedTimeout()
        {
            // Arrange
            var expectedResponse = "{\"success\": true}";
            var requestData = new { test = "value" };
            var language = "en-US";
            var timeout = 200;

            SetupMockHttpResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _betListApiClient.PostStringWithStatus("/api/test", requestData, language, timeout);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse, result.Content);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PostStringWithStatus_WithNullResponse_ShouldHandleGracefully()
        {
            // Arrange
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponseWithNullResponse();

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => 
                _betListApiClient.PostStringWithStatus("/api/test", requestData, language));
        }

        [Fact]
        public async Task PostStringWithStatus_WithContentReadError_ShouldReturnStatusCodeWithNullContent()
        {
            // Arrange
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponseWithContentReadError(HttpStatusCode.OK);

            // Act
            var result = await _betListApiClient.PostStringWithStatus("/api/test", requestData, language);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Content);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PrepareHeaders_ShouldSetCorrectHeaders()
        {
            // Arrange
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.OK, "{\"success\": true}");

            // Act
            await _betListApiClient.PostString("/api/test", requestData, language);

            // Assert
            VerifyHttpRequestHeaders();
        }

        [Fact]
        public async Task PostString_WithLargeResponse_ShouldHandleChunkedReading()
        {
            // Arrange
            var largeResponse = new string('x', 10000); // 10KB response
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.OK, largeResponse);

            // Act
            var result = await _betListApiClient.PostString("/api/test", requestData, language);

            // Assert
            Assert.Equal(largeResponse, result);
        }

        [Fact]
        public async Task PostStringWithStatus_WithLargeResponse_ShouldHandleChunkedReading()
        {
            // Arrange
            var largeResponse = new string('x', 10000); // 10KB response
            var requestData = new { test = "value" };
            var language = "en-US";

            SetupMockHttpResponse(HttpStatusCode.OK, largeResponse);

            // Act
            var result = await _betListApiClient.PostStringWithStatus("/api/test", requestData, language);

            // Assert
            Assert.Equal(largeResponse, result.Content);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        #region Helper Methods

        private void SetupMockHttpResponse(HttpStatusCode statusCode, string content)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        private void SetupMockHttpResponseWithException(Exception exception)
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(exception);
        }

        private void SetupMockHttpResponseWithNullResponse()
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpResponseMessage)null);
        }

        private void SetupMockHttpResponseWithContentReadError(HttpStatusCode statusCode)
        {
            // Create a custom HttpContent that throws an exception when ReadAsStreamAsync is called
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new ThrowingHttpContent()
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        private class ThrowingHttpContent : HttpContent
        {
            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return Task.CompletedTask;
            }

            protected override bool TryComputeLength(out long length)
            {
                length = 0;
                return true;
            }

            protected override Task<Stream> CreateContentReadStreamAsync()
            {
                throw new IOException("Content read error");
            }
        }

        private void VerifyHttpRequestHeaders()
        {
            _mockHttpMessageHandler
                .Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Headers.Contains("X-SiteDBServer") &&
                        req.Headers.Authorization != null &&
                        req.Headers.Authorization.Scheme == "Bearer" &&
                        req.Headers.Authorization.Parameter == "test-token" &&
                        req.Headers.Contains("Language")),
                    ItExpr.IsAny<CancellationToken>());
        }

        #endregion
    }
} 