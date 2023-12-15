using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.UnitTests.Common.MockClasses;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.ConsoleClient
{
    public class BetListApiClientTest
    {
        private const string APIURL = "http://localhost";
        private const string TestPath = "/ANYPATH";
        private readonly string LANG = "vi-VN";
        private readonly object TestValue = new { Name = "Alex", Code = 1 };

        [Fact]
        public async Task PostString_WhenCall_ReturnCorrectResponse()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(new MemoryStream(new byte[] { 1 }))
            };
            var httpMessageHandler = new MockHttpMessageHandler { HttpResponseMessage = httpResponse };
            var client = new HttpClient(httpMessageHandler) { BaseAddress = new Uri(APIURL) };
            var betListApiClient = new BetListApiClient(client, new Mock<IContextService>().Object);

            // Action
            var result = await betListApiClient.PostString(TestPath, TestValue, LANG).ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostStream_WhenCall_ReturnCorrectResponse()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(new MemoryStream(new byte[] { 1 }))
            };
            var httpMessageHandler = new MockHttpMessageHandler { HttpResponseMessage = httpResponse };
            var client = new HttpClient(httpMessageHandler) { BaseAddress = new Uri(APIURL) };
            var betListApiClient = new BetListApiClient(client, new Mock<IContextService>().Object);

            // Action
            var result = await betListApiClient.PostStream(TestPath, TestValue, LANG).ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostStream_NoContent_ReturnEmptyStream()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            };
            var httpMessageHandler = new MockHttpMessageHandler { HttpResponseMessage = httpResponse };
            var client = new HttpClient(httpMessageHandler) { BaseAddress = new Uri(APIURL) };
            var betListApiClient = new BetListApiClient(client, new Mock<IContextService>().Object);

            // Action
            var result = await betListApiClient.PostStream(TestPath, TestValue, LANG).ConfigureAwait(false);

            // Assert
            Assert.Equal(0, result.Length);
        }
    }
}
