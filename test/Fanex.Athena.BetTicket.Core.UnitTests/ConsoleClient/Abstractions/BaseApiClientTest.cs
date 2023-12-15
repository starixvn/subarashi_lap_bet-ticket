using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.UnitTests.Common.MockClasses;
using Newtonsoft.Json;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.ConsoleClient.Abstractions
{
    public class BaseApiClientTest
    {
        private const string Language = "en-US";
        private const string ApiUrl = "/api/Report/Action";
        private readonly Uri BaseAddress = new("http://localhost:80");

        [Theory]
        [InlineData(MediaType.Json)]
        [InlineData(MediaType.MsgPack)]
        public async Task HandleAsync_WhenCall_HttpRequestParamIsPassedCorrectly(MediaType mediaType)
        {
            // Arrange
            var httpResponse = new HttpResponseMessage();
            var httpMessageHandler = new MockHttpMessageHandler
            {
                HttpResponseMessage = httpResponse
            };

            var client = new HttpClient(httpMessageHandler) { BaseAddress = BaseAddress };
            var apiClient = new BaseApiClientTestable(client);
            var content = new
            {
                Currency = 1,
                UserName = "Test"
            };

            // Action
            var result = await apiClient.InvokeHandleAsync(ApiUrl, content, Language,
                                                           mediaType, Task.FromResult, false);

            // Assert
            Assert.Equal(httpResponse.StatusCode, result.StatusCode);
            Assert.Equal(httpResponse.Content, result.Content);
        }

        [Fact]
        public void HandleAsync_UnSuccessful_ThrowException()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(string.Empty)
            };
            var httpMessageHandler = new MockHttpMessageHandler
            {
                HttpResponseMessage = httpResponse
            };
            var client = new HttpClient(httpMessageHandler) { BaseAddress = BaseAddress };
            var apiClient = new BaseApiClientTestable(client);

            // Assert
            Assert.ThrowsAsync<ServiceException>(async () => await apiClient.InvokeHandleAsync(ApiUrl, null, Language,
                                                              MediaType.Json, Task.FromResult, false));
        }

        [Fact]
        public async Task HandleAsync_CodeBlockNull_ReturnDefault()
        {
            // Arrange
            var client = new HttpClient();
            var apiClient = new BaseApiClientTestable(client);

            // Act
            var result = await apiClient.InvokeHandleAsync<DummyObj>(ApiUrl, null, Language,
                                                                     MediaType.Json, null, false);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Post_HaveResponse_ReturnValue()
        {
            // Arrange
            var content = new DummyObj
            {
                Currency = 1,
                UserName = "Test"
            };
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(FakeContent(content))
            };
            var httpMessageHandler = new MockHttpMessageHandler { HttpResponseMessage = httpResponse };
            var client = new HttpClient(httpMessageHandler) { BaseAddress = BaseAddress };
            var betListApiClient = new BaseApiClientTestable(client);

            // Action
            var result = await betListApiClient.Post<DummyObj>(ApiUrl, null, Language).ConfigureAwait(false);

            // Assert
            Assert.Equal(content.Currency, result.Currency);
            Assert.Equal(content.UserName, result.UserName);
        }

        #region TestHelpers

        private MemoryStream FakeContent<T>(T content)
        {
            var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, 4096, true);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, content);
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private class DummyObj
        {
            public int Currency { get; set; }
            public string UserName { get; set; }
        }

        #endregion
    }
}
